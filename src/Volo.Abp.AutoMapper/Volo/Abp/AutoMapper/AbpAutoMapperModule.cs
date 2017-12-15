using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperModule : AbpModule
    {
        private static volatile bool _createdMappingsBefore;
        private static readonly object SyncObj = new object();

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAutoMapperModule>();

            var mapperAccessor = new MapperAccessor();
            services.AddSingleton<IMapperAccessor>(_ => mapperAccessor);
            services.AddSingleton<MapperAccessor>(_ => mapperAccessor);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
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

                    if (options.UseStaticMapper)
                    {
                        //We should prevent duplicate mapping in an application, since Mapper is static.
                        if (!_createdMappingsBefore)
                        {
                            Mapper.Initialize(mapperConfigurationExpression =>
                            {
                                ConfigureAll(new AbpAutoMapperConfigurationContext(mapperConfigurationExpression, scope.ServiceProvider));
                            });

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

            logger.LogDebug($"Found {types.Length} classes define auto mapping attributes:");

            foreach (var type in types)
            {
                logger.LogDebug(type.FullName);
                context.MapperConfiguration.CreateAutoAttributeMaps(type);
            }
        }
    }
}
