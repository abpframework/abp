using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Volo.Abp.Modularity;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperModule : AbpModule
    {
        //private readonly ITypeFinder _typeFinder;

        //private static volatile bool _createdMappingsBefore;
        //private static readonly object SyncObj = new object();
        
        //public AbpAutoMapperModule(ITypeFinder typeFinder)
        //{
        //    _typeFinder = typeFinder;
        //}

        //public override void PreInitialize()
        //{
        //    IocManager.Register<IAbpAutoMapperConfiguration, AbpAutoMapperConfiguration>();

        //    Configuration.ReplaceService<ObjectMapping.IObjectMapper, AutoMapperObjectMapper>();

        //    Configuration.Modules.AbpAutoMapper().Configurators.Add(CreateCoreMappings);
        //}

        //public override void PostInitialize()
        //{
        //    CreateMappings();
        //}

        //private void CreateMappings()
        //{
        //    lock (SyncObj)
        //    {
        //        Action<IMapperConfigurationExpression> configurer = configuration =>
        //        {
        //            FindAndAutoMapTypes(configuration);
        //            foreach (var configurator in Configuration.Modules.AbpAutoMapper().Configurators)
        //            {
        //                configurator(configuration);
        //            }
        //        };

        //        if (Configuration.Modules.AbpAutoMapper().UseStaticMapper)
        //        {
        //            //We should prevent duplicate mapping in an application, since Mapper is static.
        //            if (!_createdMappingsBefore)
        //            {
        //                Mapper.Initialize(configurer);
        //                _createdMappingsBefore = true;
        //            }

        //            IocManager.IocContainer.Register(
        //                Component.For<IMapper>().Instance(Mapper.Instance).LifestyleSingleton()
        //            );
        //        }
        //        else
        //        {
        //            var config = new MapperConfiguration(configurer);
        //            IocManager.IocContainer.Register(
        //                Component.For<IMapper>().Instance(config.CreateMapper()).LifestyleSingleton()
        //            );
        //        }
        //    }
        //}

        //private void FindAndAutoMapTypes(IMapperConfigurationExpression configuration)
        //{
        //    var types = _typeFinder.Find(type =>
        //        {
        //            var typeInfo = type.GetTypeInfo();
        //            return typeInfo.IsDefined(typeof(AutoMapAttribute)) ||
        //                   typeInfo.IsDefined(typeof(AutoMapFromAttribute)) ||
        //                   typeInfo.IsDefined(typeof(AutoMapToAttribute));
        //        }
        //    );

        //    Logger<>.DebugFormat("Found {0} classes define auto mapping attributes", types.Length);

        //    foreach (var type in types)
        //    {
        //        Logger<>.Debug(type.FullName);
        //        configuration.CreateAutoAttributeMaps(type);
        //    }
        //}

        //private void CreateCoreMappings(IMapperConfigurationExpression configuration)
        //{
        //    var localizationContext = IocManager.Resolve<ILocalizationContext>();

        //    configuration.CreateMap<ILocalizableString, string>().ConvertUsing(ls => ls?.Localize(localizationContext));
        //    configuration.CreateMap<LocalizableString, string>().ConvertUsing(ls => ls == null ? null : localizationContext.LocalizationManager.GetString(ls));
        //}
    }
}
