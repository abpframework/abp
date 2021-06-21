using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account.Web
{
    [DependsOn(
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpExceptionHandlingModule)
        )]
    public class AbpAccountWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AccountResource), typeof(AbpAccountWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountWebModule>();
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpAccountUserMenuContributor());
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new AccountModuleToolbarContributor());
            });

            ConfigureProfileManagementPage();

            context.Services.AddAutoMapperObjectMapper<AbpAccountWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpAccountWebAutoMapperProfile>(validate: true);
            });
        }

        private void ConfigureProfileManagementPage()
        {
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Account/Manage");
            });

            Configure<ProfileManagementPageOptions>(options =>
            {
                options.Contributors.Add(new AccountProfileManagementPageContributor());
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options.ScriptBundles
                    .Configure(typeof(ManageModel).FullName,
                        configuration =>
                        {
                            configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/Password/Default.js");
                            configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Default.js");
                        });
            });

        }
    }
}
