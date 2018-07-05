using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Web.Localization;
using Volo.Abp.Account.Web.Settings;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account.Web
{
    [DependsOn(typeof(AbpIdentityDomainModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiThemeSharedModule))]
    public class AbpAccountWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AccountResource), typeof(AbpAccountWebModule).Assembly);
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<AccountSettingDefinitionProvider>();
            });

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountWebModule>("Volo.Abp.Account.Web");
            });

            services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpAccountUserMenuContributor());
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AccountResource>("en")
                    .AddVirtualJson("/Localization/Resources/AbpAccount/Web")
                    .AddBaseTypes(typeof(AbpUiResource), typeof(AbpValidationResource));
            });
            
            services.Configure<ToolbarOptions>(options =>
            {
                options.Contributors.Add(new AccountModuleToolbarContributor());
            });

            services.AddAssemblyOf<AbpAccountWebModule>();
        }
    }
}
