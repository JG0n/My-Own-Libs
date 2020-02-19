using System;
using System.Collections.Generic;
using System.Linq.Expressions;

abstract class Mapper
{
    public static Mapper Empty = new Mapper<object, object>();

    public Mapper(Type sourceType, Type destination)
    {
        SourceType = sourceType;
        Destination = destination;
        PropMappers = new Dictionary<string, Func<object, object>>();
    }

    public Type SourceType { get; }
    public Type Destination { get; }
    public Dictionary<string, Func<object, object>> PropMappers { get; }
}

class Mapper<TSource, TTarget> : Mapper
    where TSource : class
    where TTarget : class
{
    public Mapper()
        : base(typeof(TSource), typeof(TTarget))
    {
    }

    public Mapper<TSource, TTarget> ForMember<TResult>(
        Expression<Func<TTarget, TResult>> toExp,
        Func<TSource, object> fromExp)
    {
        var toMemberExp = toExp.Body as MemberExpression;

        PropMappers.Add(toMemberExp.Member.Name, o => fromExp((TSource)o));
        
        return this;
    }
}