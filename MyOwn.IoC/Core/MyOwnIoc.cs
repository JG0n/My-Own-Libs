using System.Reflection;


/// <summary>
/// Implementation of a simple inversion of control container
/// </summary>
public class MyOwnIoc
{
    private Dictionary<string, Service> services = [];
    ConsoleLogger logger = new();

    /// <summary>
    /// Register a transient service
    /// </summary>
    public void Register<TInterface, TImplementation>()
    {
        if (!services.ContainsKey(typeof(TInterface).FullName!))
        {
            services[typeof(TInterface).FullName!] =
                new Service(typeof(TImplementation), Lifetime.Transient);
        }
    }

    /// <summary>
    /// Register a singleton service
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <returns></returns>
    public void RegisterSingleton<TInterface, TImplementation>()
    {
        if (!services.ContainsKey(typeof(TInterface).FullName!))
        {
            services[typeof(TInterface).FullName!] =
                new Service(typeof(TImplementation), Lifetime.Singleton);
        }
    }

    /// <summary>
    /// Resolve a service
    /// </summary>
    /// <typeparam name="T">The type of the service that is being resolved</typeparam>
    /// <returns>An instance of <paramref name="T"/> if it exists</returns>
    public T Resolve<T>() => (T)Resolve(typeof(T));

    private object Resolve(Type resolveType)
    {
        try
        {
            if (!services.TryGetValue(resolveType.FullName!, out Service? service)
                    && service == null)
            {
                throw new Exception($"Could not resolve {resolveType.FullName}");
            }

            switch (service.Lifetime)
            {
                case Lifetime.Scope:
                    throw new NotImplementedException(
                            $"Lifetime {nameof(service.Lifetime)} does not exist");
                case Lifetime.Singleton:
                    return ResolveSingleton(service);
                case Lifetime.Transient:
                    return ResolveTransient(service);
                default:
                    throw new NotImplementedException(
                            $"Lifetime {nameof(service.Lifetime)} does not exist");

            }
        }
        catch (Exception ex)
        {
            logger.Log(ex.Message);
            throw;
        }
    }

    private object ResolveTransient(Service service)
    {
        return InstanciateRegisteredService(service);
    }

    private object ResolveSingleton(Service service)
    {
        if (service.Instance == null)
        {
            service.Instance = InstanciateRegisteredService(service);
        }
        return service.Instance;
    }

    private object InstanciateRegisteredService(Service service)
    {
        Type type = Type.GetType(service.Class)
            ?? throw new Exception($"Class {service.Class} does not exist");

        foreach (ConstructorInfo constructor in type.GetConstructors())
        {
            List<object> parameters = [];
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            foreach (ParameterInfo param in constructorParameters)
            {
                try
                {
                    parameters.Add(Resolve(param.ParameterType));
                }
                catch (Exception ex)
                {
                    logger.Log(ex.Message);
                    parameters = [];
                    break;
                }
            }

            if (parameters.Count == constructorParameters.Length)
            {
                return constructor.Invoke(parameters.ToArray());
            }
        }


        throw new Exception($"Coud not resolve service of type {service.Class}");
    }
}
