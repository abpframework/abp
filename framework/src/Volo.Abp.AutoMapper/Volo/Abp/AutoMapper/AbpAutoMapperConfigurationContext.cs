using System;
using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperConfigurationContext : IAbpAutoMapperConfigurationContext
    {
        public IMapperConfigurationExpression MapperConfiguration { get; }
        public IServiceProvider ServiceProvider { get; }

        public AbpAutoMapperConfigurationContext(
            IMapperConfigurationExpression mapperConfigurationExpression, 
            IServiceProvider serviceProvider)
        {
            MapperConfiguration = mapperConfigurationExpression;
            ServiceProvider = serviceProvider;
        }
    }
}