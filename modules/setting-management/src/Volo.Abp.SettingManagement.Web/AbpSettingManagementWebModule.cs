using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.SettingManagement.Web.Settings;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.SettingManagement.Web
{
    [DependsOn(
        typeof(AbpSettingManagementHttpApiModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpSettingManagementDomainSharedModule)
        )]
    public class AbpSettingManagementWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpSettingManagementWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new SettingManagementMainMenuContributor());
            });

            Configure<SettingManagementPageOptions>(options =>
            {
                options.Contributors.Add(new EmailingPageContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpSettingManagementWebModule>();
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options.ScriptBundles
                    .Configure(typeof(IndexModel).FullName,
                        configuration =>
                        {
                            configuration.AddFiles("/Pages/SettingManagement/Components/EmailSettingGroup/Default.js");
                        });
            });
        }
    }
}
