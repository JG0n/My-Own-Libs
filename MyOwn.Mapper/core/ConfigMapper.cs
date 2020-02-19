using System.Collections.Generic;
using System.Linq;

class ConfigMapper
{
    private readonly List<Mapper> mappers = new List<Mapper>();

    public Mapper<TSource, TTarget> CreateMap<TSource, TTarget>()
        where TSource : class
        where TTarget : class
    {
        var mapper = new Mapper<TSource, TTarget>();
        mappers.Add(mapper);
        return mapper;
    }

    public Dictionary<string, Mapper> Build()
        => mappers.ToDictionary(m => m.SourceType.FullName);
}