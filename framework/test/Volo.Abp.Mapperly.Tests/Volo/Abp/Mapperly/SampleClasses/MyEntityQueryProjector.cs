using System.Linq;
using Riok.Mapperly.Abstractions;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Mapperly.SampleClasses;

[Mapper]
public partial class MyEntityQueryProjector : IQueryProjectionMapper<MyEntity, MyEntityDto>
{
    public partial IQueryable<MyEntityDto> ProjectTo(IQueryable<MyEntity> source);
}
