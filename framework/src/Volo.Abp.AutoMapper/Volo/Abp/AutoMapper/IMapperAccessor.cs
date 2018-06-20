using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public interface IMapperAccessor
    {
        IMapper Mapper { get; }
    }
}
