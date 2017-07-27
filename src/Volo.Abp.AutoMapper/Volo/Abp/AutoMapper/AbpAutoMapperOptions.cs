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
        IMapperConfigurationExpression MapperConfigurationExpression { get; }

        IServiceProvider ServiceProvider { get; }
    }

    public class AbpAutoMapperConfigurationContext : IAbpAutoMapperConfigurationContext
    {
        public IMapperConfigurationExpression MapperConfigurationExpression { get; }
        public IServiceProvider ServiceProvider { get; }

        public AbpAutoMapperConfigurationContext(
            IMapperConfigurationExpression mapperConfigurationExpression, 
            IServiceProvider serviceProvider)
        {
            MapperConfigurationExpression = mapperConfigurationExpression;
            ServiceProvider = serviceProvider;
        }
    }
}