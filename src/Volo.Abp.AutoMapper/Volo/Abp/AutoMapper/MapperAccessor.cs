using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public class MapperAccessor : IMapperAccessor
    {
        public IMapper Mapper { get; set; }
    }
}