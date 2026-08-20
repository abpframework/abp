using System.Linq;
using Riok.Mapperly.Abstractions;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Mapperly.SampleClasses;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MyEntityQueryProjector : IQueryableMapper<MyEntity, MyEntityDto>
{
    public partial IQueryable<MyEntityDto> ProjectTo(IQueryable<MyEntity> source);
}
