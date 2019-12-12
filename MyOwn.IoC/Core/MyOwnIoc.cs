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
            logger.Error($" Type not registered <{resolveTypeName}>");
            logger.Log("Terminating...");
            Environment.Exit(-1);
            return null;
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
            logger.Warning($"Singleton created <{resolveTypeName},{nameTypeToResolve}>");

            object obj = Activator.CreateInstance(registrations[resolveTypeName].Type);
            registrations[resolveTypeName].Instance = obj;
        }

        if (registrations[resolveTypeName].IsSingleton)
        {
            logger.Warning($"Resolved Singleton <{resolveTypeName},{nameTypeToResolve}>");

            return registrations[resolveTypeName].Instance;
        }

        var ctors = registrations[resolveTypeName].Type
            .GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length);

        var args = new List<object>();
        foreach (var ctor in ctors)
        {
            foreach (var param in ctor.GetParameters())
            {
                args.Add(Resolve(param.ParameterType));
            }
        }

        logger.Warning($"Resolved <{resolveTypeName},{nameTypeToResolve}>");
        return Activator.CreateInstance(registrations[resolveTypeName].Type, args.ToArray());
    }
}