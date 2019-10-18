using System.Collections.Generic;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain;
using Volo.Abp.Features;
using Volo.Abp.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Security;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Volo.Abp.Application
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpSecurityModule),
        typeof(AbpObjectMappingModule),
        typeof(AbpValidationModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpHttpAbstractionsModule),
        typeof(AbpSettingsModule),
        typeof(AbpFeaturesModule)
        )]
    public class AbpDddApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpApiDescriptionModelOptions>(options =>
            {
                //TODO: Should we move related items to their own projects?
                options.IgnoredInterfaces.AddIfNotContains(typeof(IRemoteService));
                options.IgnoredInterfaces.AddIfNotContains(typeof(IApplicationService));
                options.IgnoredInterfaces.AddIfNotContains(typeof(IUnitOfWorkEnabled));
            });
        }
    }
}
