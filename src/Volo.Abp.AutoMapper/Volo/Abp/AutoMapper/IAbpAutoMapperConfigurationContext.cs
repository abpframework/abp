using System;
using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public interface IAbpAutoMapperConfigurationContext
    {
        IMapperConfigurationExpression MapperConfiguration { get; }

        IServiceProvider ServiceProvider { get; }
    }
}