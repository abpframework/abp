using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.DependencyInjection;

namespace Volo.Abp.AutoMapper
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    public class AutoMapperObjectMapper : DefaultObjectMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperObjectMapper(IMapperAccessor mapper, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _mapper = mapper.Mapper;
        }

        protected override TDestination AutoMap<TSource, TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        protected override TDestination AutoMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
