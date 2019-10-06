using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AutoMapper
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    public class AutoMapperAutoObjectMappingProvider : IAutoObjectMappingProvider
    {
        protected IMapperAccessor MapperAccessor { get; }

        public AutoMapperAutoObjectMappingProvider(IMapperAccessor mapperAccessor)
        {
            MapperAccessor = mapperAccessor;
        }

        public virtual TDestination Map<TSource, TDestination>(object source)
        {
            return MapperAccessor.Mapper.Map<TDestination>(source);
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return MapperAccessor.Mapper.Map(source, destination);
        }
    }
}
