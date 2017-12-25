using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.ApiVersioning;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp
{
    [DependsOn(typeof(AbpLocalizationModule))]
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    [DependsOn(typeof(AbpApiVersioningAbstractionsModule))]
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    [DependsOn(typeof(AbpObjectMappingModule))]
    [DependsOn(typeof(AbpValidationModule))]
    public class AbpCommonModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnRegistred(UnitOfWorkInterceptorRegistrar.RegisterIfNeeded);
            services.OnRegistred(ValidationInterceptorRegistrar.RegisterIfNeeded);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);

            services.AddAssemblyOf<AbpCommonModule>();
        }
    }
}
