using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.ApiVersioning;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Security;
using Volo.Abp.Session;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp
{
    [DependsOn(typeof(AbpAuthorizationModule))]
    [DependsOn(typeof(AbpValidationModule))]
    [DependsOn(typeof(AbpLocalizationModule))]
    [DependsOn(typeof(AbpObjectMappingModule))]
    [DependsOn(typeof(AbpSecurityModule))]
    [DependsOn(typeof(AbpSessionModule))]
    [DependsOn(typeof(AbpSettingsModule))]
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpJsonModule))]
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    [DependsOn(typeof(AbpApiVersioningAbstractionsModule))]
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    [DependsOn(typeof(AbpCachingModule))]
    public class AbpCommonModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpCommonModule>();
        }
    }
}
