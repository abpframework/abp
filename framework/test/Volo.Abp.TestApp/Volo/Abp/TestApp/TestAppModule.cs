using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Application;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.TestApp.Application.Dto;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Threading;

namespace Volo.Abp.TestApp;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutoMapperModule)
    )]
public class TestAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureAutoMapper();
        ConfigureDistributedEventBus();
        
        context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache, TestMemoryDistributedCache>());
        context.Services.AddEntityCache<Product, Guid>();
        context.Services.AddEntityCache<Product, ProductCacheItem, Guid>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.Configurators.Add(ctx =>
            {
                ctx.MapperConfiguration.CreateMap<Person, PersonDto>().ReverseMap();
                ctx.MapperConfiguration.CreateMap<Phone, PhoneDto>().ReverseMap();
            });

            options.AddMaps<TestAppModule>();
        });
    }

    private void ConfigureDistributedEventBus()
    {
        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<Person>();
            options.EtoMappings.Add<Person, PersonEto>();
        });
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(() => scope.ServiceProvider
                .GetRequiredService<TestDataBuilder>()
                .BuildAsync());
        }
    }
}
