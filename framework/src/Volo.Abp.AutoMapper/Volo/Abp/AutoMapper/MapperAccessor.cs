using AutoMapper;

namespace Volo.Abp.AutoMapper;

internal class MapperAccessor : IMapperAccessor
{
    public IMapper Mapper { get; set; }
}
