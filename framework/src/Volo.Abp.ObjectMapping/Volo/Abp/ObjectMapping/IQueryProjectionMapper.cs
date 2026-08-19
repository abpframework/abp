using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping;

public interface IQueryProjectionMapper<TSource, TDestination> : ITransientDependency
{
    IQueryable<TDestination> ProjectTo(IQueryable<TSource> source);
}

//[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
//public partial class GetSectorMapper : IQueryProjectionMapper<Sector, GetSectorsDto>
//{
//    public partial IQueryable<GetSectorsDto> ProjectTo(IQueryable<Sector> source);
//}