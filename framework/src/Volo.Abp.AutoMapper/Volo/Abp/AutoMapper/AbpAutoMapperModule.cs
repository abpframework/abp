using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AutoMapper
{
    [DependsOn(typeof(AbpObjectMappingModule))]
    public class AbpAutoMapperModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper();

            var mapperAccessor = new MapperAccessor();
            context.Services.AddSingleton<IMapperAccessor>(_ => mapperAccessor);
            context.Services.AddSingleton<MapperAccessor>(_ => mapperAccessor);
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            CreateMappings(context.ServiceProvider);
        }

        private void CreateMappings(IServiceProvider serviceProvider)
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
                        config.AssertConfigurationIsValid(((Profile)Activator.CreateInstance(profileType)).ProfileName);
                    }
                }

                var mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression =>
                {
                    ConfigureAll(new AbpAutoMapperConfigurationContext(mapperConfigurationExpression, scope.ServiceProvider));
                });

                ValidateAll(mapperConfiguration);

                scope.ServiceProvider.GetRequiredService<MapperAccessor>().Mapper = mapperConfiguration.CreateMapper();
            }
        }
    }
}
