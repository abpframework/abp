using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain;
using Volo.Abp.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Security;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Volo.Abp.Application
{
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpSecurityModule))]
    [DependsOn(typeof(AbpObjectMappingModule))]
    [DependsOn(typeof(AbpValidationModule))]
    [DependsOn(typeof(AbpAuthorizationModule))]
    [DependsOn(typeof(AbpHttpAbstractionsModule))]
    public class AbpDddApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<ApiDescriptionModelOptions>(options =>
            {
                options.IgnoredInterfaces.AddIfNotContains(typeof(IRemoteService));
                options.IgnoredInterfaces.AddIfNotContains(typeof(IApplicationService));
                options.IgnoredInterfaces.AddIfNotContains(typeof(IUnitOfWorkEnabled)); //TODO: Move to it's own module if possible?
                options.IgnoredInterfaces.AddIfNotContains(typeof(IAuthorizationEnabled)); //TODO: Move to it's own module if possible?
            });
        }
    }
}
