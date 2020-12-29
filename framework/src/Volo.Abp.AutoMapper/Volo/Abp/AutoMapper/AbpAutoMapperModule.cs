using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AutoMapper
{
    [DependsOn(
        typeof(AbpObjectMappingModule),
        typeof(AbpObjectExtendingModule),
        typeof(AbpAuditingModule)
    )]
    public class AbpAutoMapperModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new AbpAutoMapperConventionalRegistrar());
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper();

            var mapperAccessor = new MapperAccessor();
            context.Services.AddSingleton(_ => mapperAccessor);
            context.Services.Add(new ServiceDescriptor(typeof(IMapperAccessor), CreateMappings, ServiceLifetime.Singleton));
        }

        private IMapperAccessor CreateMappings(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var options = scope.ServiceProvider.GetRequiredService<IOptions<AbpAutoMapperOptions>>().Value;

                void ConfigureAll(IAbpAutoMapperConfigurationContext ctx)
                {
                    foreach (var configurator in options.Configurators)
                    {
                        configurator(ctx);
                    }
                }

                void ValidateAll(IConfigurationProvider config)
                {
                    foreach (var profileType in options.ValidatingProfiles)
                    {
                        config.AssertConfigurationIsValid(((Profile) Activator.CreateInstance(profileType)).ProfileName);
                    }
                }

                var mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression =>
                {
                    ConfigureAll(new AbpAutoMapperConfigurationContext(mapperConfigurationExpression, scope.ServiceProvider));
                });

                ValidateAll(mapperConfiguration);

                var mapperAccessor = scope.ServiceProvider.GetRequiredService<MapperAccessor>();
                mapperAccessor.Mapper = new Mapper(mapperConfiguration, serviceProvider.GetService);

                return mapperAccessor;
            }
        }
    }
}
