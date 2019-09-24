using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AutoMapper
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    public class AutoMapperObjectMapper : DefaultObjectMapper
    {
        protected IMapperAccessor MapperAccessor { get; }

        public AutoMapperObjectMapper(IMapperAccessor mapperAccessor, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            MapperAccessor = mapperAccessor;
        }

        protected override TDestination AutoMap<TSource, TDestination>(object source)
        {
            return MapperAccessor.Mapper.Map<TDestination>(source);
        }

        protected override TDestination AutoMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            return MapperAccessor.Mapper.Map(source, destination);
        }

        protected override IQueryable<TDestination> AutoProjectTo<TDestination>(IQueryable source)
        {
            return MapperAccessor.Mapper.ProjectTo<TDestination>(source);
        }
    }
}
