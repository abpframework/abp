using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.EventBus;
using Volo.Abp.Guids;
using Volo.Abp.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Volo.Abp
{
    //TODO: Consider to split this DDD package by layers!

    [DependsOn(typeof(AbpGuidsModule))]
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpObjectMappingModule))]
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    [DependsOn(typeof(AbpThreadingModule))]
    [DependsOn(typeof(AbpEventBusModule))]
    [DependsOn(typeof(AbpValidationModule))]
    [DependsOn(typeof(AbpAuthorizationModule))]
    [DependsOn(typeof(AbpHttpAbstractionsModule))]
    public class AbpDddModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnRegistred(UnitOfWorkInterceptorRegistrar.RegisterIfNeeded);
            services.OnRegistred(ValidationInterceptorRegistrar.RegisterIfNeeded);
            services.OnRegistred(AuthorizationInterceptorRegistrar.RegisterIfNeeded);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiDescriptionModelOptions>(options =>
            {
                options.IgnoredInterfaces.AddIfNotContains(typeof(IRemoteService));
                options.IgnoredInterfaces.AddIfNotContains(typeof(IApplicationService));
            });

            services.AddAssemblyOf<AbpDddModule>();
        }
    }
}
