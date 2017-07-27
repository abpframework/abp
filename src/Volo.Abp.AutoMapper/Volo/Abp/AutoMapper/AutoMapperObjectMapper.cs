using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp.AutoMapper
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    public class AutoMapperObjectMapper : Volo.Abp.ObjectMapping.IObjectMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperObjectMapper(IMapperAccessor mapper)
        {
            _mapper = mapper.Mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
