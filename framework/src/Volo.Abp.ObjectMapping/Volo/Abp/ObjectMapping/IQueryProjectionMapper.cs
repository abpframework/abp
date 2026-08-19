using System.Linq;
using Volo.Abp.DependencyInjection;
namespace Volo.Abp.ObjectMapping;

public interface IQueryProjectionMapper<TSource, TDestination> : ITransientDependency
{
    IQueryable<TDestination> ProjectTo(IQueryable<TSource> source);
}