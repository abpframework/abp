using System;
using System.Collections.Generic;
using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperOptions
    {
        public List<Action<IAbpAutoMapperConfigurationContext>> Configurators { get; }

        public bool UseStaticMapper { get; set; }

        public AbpAutoMapperOptions()
        {
            UseStaticMapper = true;
            Configurators = new List<Action<IAbpAutoMapperConfigurationContext>>();
        }
    }

    public interface IAbpAutoMapperConfigurationContext
    {
        IMapperConfigurationExpression MapperConfiguration { get; }

        IServiceProvider ServiceProvider { get; }
    }

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