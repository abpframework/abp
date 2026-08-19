using System.Linq;

namespace Volo.Abp.ObjectMapping;

public abstract class QueryProjectionMapper<TSource, TDestination> : IQueryProjectionMapper<TSource, TDestination>
{
    public abstract IQueryable<TDestination> ProjectTo(IQueryable<TSource> source);
}