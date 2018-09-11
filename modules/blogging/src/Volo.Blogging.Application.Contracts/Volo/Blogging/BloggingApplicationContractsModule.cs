using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    [DependsOn(typeof(BloggingDomainSharedModule))]
    public class BloggingApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BloggingResource>()
                    .AddVirtualJson("/Localization/Resources/Blogging/ApplicationContracts");
            });

            context.Services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<BloggingPermissionDefinitionProvider>();
            });
        }
    }
}
