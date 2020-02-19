using System;
using System.Collections.Generic;

class MyOwnMapper
{
    private readonly ConsoleLogger logger = new ConsoleLogger();
    private readonly Dictionary<string, Mapper> entitiesMapper;

    public MyOwnMapper(Action<ConfigMapper> configFunc)
    {
        var config = new ConfigMapper();
        configFunc(config);
        entitiesMapper = config.Build();
    }

    public TTarget Map<TTarget>(object obj)
        where TTarget : new()
    {
        var sourceType = obj.GetType();

        object targetObj = PerformMapping(obj, sourceType, typeof(TTarget));

        return (TTarget)targetObj;
    }

    public TTarget Map<TSource, TTarget>(TTarget obj)
        where TTarget : new() => Map<TTarget>(obj);

    private object PerformMapping(object sourceObj, Type sourceType, Type targetObjType)
    {
        object targetObj = Activator.CreateInstance(targetObjType);

        var typeMappers = entitiesMapper.ContainsKey(sourceType.FullName)
                ? entitiesMapper[sourceType.FullName]
                : Mapper.Empty;

        if (typeMappers == Mapper.Empty)
            logger.Warning("Mapper not registered");
        else
            logger.Info($"Mapper for type '{sourceType}' found");

        foreach (var targetProp in targetObjType.GetProperties())
        {
            var srcProp = sourceType.GetProperty(targetProp.Name);

            if (srcProp != null && srcProp.PropertyType == targetProp.PropertyType)
            {
                logger.Info($"Setting property '{targetProp.Name}' from '{sourceType}' to '{targetObjType}'");
                targetProp.SetValue(targetObj, srcProp.GetValue(sourceObj));
            }
            else if (typeMappers.PropMappers.ContainsKey(targetProp.Name))
            {
                var propMapper = typeMappers.PropMappers[targetProp.Name];

                logger.Info($"Executing mapper function for property '{targetProp.Name}'");
                object targetValue = propMapper(sourceObj);

                targetProp.SetValue(targetObj, targetValue);
            }
            else if (!targetProp.PropertyType.IsPrimitive &&
                targetProp.PropertyType.Name != "String")
            {
                logger.Warning($"Complex property '{targetProp.Name}' of type '{targetProp.PropertyType}'");
                object value = PerformMapping(srcProp.GetValue(sourceObj), srcProp.PropertyType, targetProp.PropertyType);
                targetProp.SetValue(targetObj, value);
            }
            else
            {
                logger.Warning($"Property '{targetProp.Name}' will not be mapped");
            }
        }

        return targetObj;
    }
}