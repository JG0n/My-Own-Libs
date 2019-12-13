using System;
using System.Linq;
using System.Collections.Generic;
class MyOwnIoc
{
    ConsoleLogger logger = new ConsoleLogger();
    Dictionary<string, Registration> registrations = new Dictionary<string, Registration>();

    public void Register<TInterface, TImplementation>()
    {
        string interfaceName = typeof(TInterface).FullName;
        string implementation = typeof(TImplementation).FullName;

        if (!registrations.ContainsKey(interfaceName))
            registrations.Add(interfaceName, new Registration(implementation));
    }

    public void RegisterSingleton<TInterface, TImplementation>()
    {
        string interfaceName = typeof(TInterface).FullName;
        string implementation = typeof(TImplementation).FullName;

        if (!registrations.ContainsKey(interfaceName))
            registrations.Add(interfaceName, new Registration(implementation, true));
    }

    public T Resolve<T>() => (T)Resolve(typeof(T));

    private object Resolve(Type resolveType)
    {
        string resolveTypeName = resolveType.FullName;

        if (!registrations.ContainsKey(resolveTypeName))
        {
            logger.Error($"Type not registered <{resolveTypeName}>");
            throw new NullResolutionException();
        }

        string nameTypeToResolve = registrations[resolveTypeName].TypeName;
        if (registrations[resolveTypeName].Type == null)
        {
            var typeToResolve = Type.GetType(nameTypeToResolve);
            registrations[resolveTypeName].Type = typeToResolve;
        }

        if (registrations[resolveTypeName].IsSingleton &&
            registrations[resolveTypeName].Instance == null)
        {
            logger.Warning($"New instance created of type <{nameTypeToResolve}>");
            object objInstance = Activator.CreateInstance(registrations[resolveTypeName].Type);
            registrations[resolveTypeName].Instance = objInstance;
        }

        if (registrations[resolveTypeName].IsSingleton)
        {
            logger.Info($"Resolved Singleton <{resolveTypeName},{nameTypeToResolve}>");
            return registrations[resolveTypeName].Instance;
        }

        var ctors = registrations[resolveTypeName].Type
            .GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length);

        var args = new List<object>();
        bool resolutionCompleted = false;
        foreach (var ctor in ctors)
        {
            resolutionCompleted = false;
            var ctorParams = ctor.GetParameters();
            foreach (var param in ctorParams)
            {
                try
                {
                    args.Add(Resolve(param.ParameterType));
                }
                catch (NullResolutionException)
                {
                    args.Clear();
                    logger.Warning($"Moving to next ctor.");
                    break; // try another ctor
                }
            }

            if (args.Count == ctorParams.Count())
            {
                resolutionCompleted = true;
                break;
            }
        }

        if (!resolutionCompleted)
        {
            logger.Error($"Can't resolve {resolveTypeName}");
            logger.Log("Terminating...");
            Environment.Exit(-1);
        }

        object obj = Activator.CreateInstance(registrations[resolveTypeName].Type, args.ToArray());
        logger.Warning($"New instance created of type <{nameTypeToResolve}>");
        logger.Info($"Resolved <{resolveTypeName},{nameTypeToResolve}>");
        return obj;
    }
}