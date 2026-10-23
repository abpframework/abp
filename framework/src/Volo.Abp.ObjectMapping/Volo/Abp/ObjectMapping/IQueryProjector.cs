using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping;

/// <summary>
/// Maps a query to another.
/// Implement this interface to project a query on the data store side, instead of loading the
/// source objects into the memory and mapping them one by one.
/// Implement it once for a source and destination pair. Use the ReplaceServices option of the
/// DependencyAttribute to replace an existing implementation.
/// </summary>
/// <typeparam name="TSource">Type of the source objects</typeparam>
/// <typeparam name="TDestination">Type of the destination objects</typeparam>
public interface IQueryProjector<TSource, TDestination> : ITransientDependency
{
    /// <summary>
    /// Projects the given query. The returned query must be built on top of it and must keep its order,
    /// with a single destination object for each source object, using expressions the query provider can
    /// translate. The caller may have already sorted, paged or counted the source query.
    /// </summary>
    /// <param name="source">The query to project</param>
    IQueryable<TDestination> ProjectTo(IQueryable<TSource> source);
}
