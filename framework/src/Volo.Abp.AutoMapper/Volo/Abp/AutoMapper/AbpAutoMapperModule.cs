using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Reflection;

namespace Volo.Abp.AutoMapper
{
    [DependsOn(typeof(AbpObjectMappingModule))]
    public class AbpAutoMapperModule : AbpModule
    {
        private static volatile bool _createdMappingsBefore;
        private static readonly object SyncObj = new object();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
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
            lock (SyncObj)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var options = scope.ServiceProvider.GetRequiredService<IOptions<AbpAutoMapperOptions>>().Value;

                    void ConfigureAll(IAbpAutoMapperConfigurationContext ctx)
                    {
                        FindAndAutoMapTypes(ctx);
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

                    if (options.UseStaticMapper)
                    {
                        //We should prevent duplicate mapping in an application, since Mapper is static.
                        if (!_createdMappingsBefore)
                        {
                            Mapper.Initialize(mapperConfigurationExpression =>
                            {
                                ConfigureAll(new AbpAutoMapperConfigurationContext(mapperConfigurationExpression, scope.ServiceProvider));
                            });

                            ValidateAll(Mapper.Configuration);

                            _createdMappingsBefore = true;
                        }

                        scope.ServiceProvider.GetRequiredService<MapperAccessor>().Mapper = Mapper.Instance;
                    }
                    else
                    {
                        var config = new MapperConfiguration(mapperConfigurationExpression =>
                        {
                            ConfigureAll(new AbpAutoMapperConfigurationContext(mapperConfigurationExpression, scope.ServiceProvider));
                        });

                        ValidateAll(config);

                        scope.ServiceProvider.GetRequiredService<MapperAccessor>().Mapper = config.CreateMapper();
                    }
                }
            }
        }

        private void FindAndAutoMapTypes(IAbpAutoMapperConfigurationContext context)
        {
            //TODO: AutoMapping (by attributes) can be optionally enabled/disabled.

            var typeFinder = context.ServiceProvider.GetRequiredService<ITypeFinder>();
            var logger = context.ServiceProvider.GetRequiredService<ILogger<AbpAutoMapperModule>>();

            var types = typeFinder.Types.Where(type =>
                {
                    var typeInfo = type.GetTypeInfo();
                    return typeInfo.IsDefined(typeof(AutoMapAttribute)) ||
                           typeInfo.IsDefined(typeof(AutoMapFromAttribute)) ||
                           typeInfo.IsDefined(typeof(AutoMapToAttribute));
                }
            ).ToArray();

            if (types.Length <= 0)
            {
                logger.LogDebug($"No class found with auto mapping attributes.");
            }
            else
            {
                logger.LogDebug($"Found {types.Length} classes define auto mapping attributes.");
                foreach (var type in types)
                {
                    logger.LogDebug(type.FullName);
                    context.MapperConfiguration.CreateAutoAttributeMaps(type);
                }
            }
        }
    }
}
