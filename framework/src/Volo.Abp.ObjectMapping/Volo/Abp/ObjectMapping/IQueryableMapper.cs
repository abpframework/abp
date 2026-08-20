using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping;

/// <summary>
/// Maps a query to another.
/// Implement this interface to project a query on the data store side, instead of loading the
/// source objects into the memory and mapping them one by one.
/// </summary>
/// <typeparam name="TSource">Type of the source objects</typeparam>
/// <typeparam name="TDestination">Type of the destination objects</typeparam>
public interface IQueryableMapper<TSource, TDestination> : ITransientDependency
{
    /// <summary>
    /// Projects the given query. The returned query must be built on top of it, otherwise the
    /// query provider can not translate the projection.
    /// </summary>
    /// <param name="source">The query to project</param>
    IQueryable<TDestination> ProjectTo(IQueryable<TSource> source);
}
