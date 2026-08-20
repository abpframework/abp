using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping;

/// <summary>
/// Projects a query of <typeparamref name="TSource"/> objects to a query of
/// <typeparamref name="TDestination"/> objects.
/// Implement this interface to let the query provider translate the projection into the data
/// store's own query language, instead of loading the source objects into the memory.
/// </summary>
/// <typeparam name="TSource">Type of the source objects</typeparam>
/// <typeparam name="TDestination">Type of the destination objects</typeparam>
public interface IQueryProjectionMapper<TSource, TDestination> : ITransientDependency
{
    /// <summary>
    /// Projects the given query to a query of <typeparamref name="TDestination"/> objects.
    /// The returned query must be built on top of <paramref name="source"/>, so the query
    /// provider can still translate it.
    /// </summary>
    /// <param name="source">The query to project</param>
    IQueryable<TDestination> ProjectTo(IQueryable<TSource> source);
}
