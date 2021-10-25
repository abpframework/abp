using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Threading;

namespace Volo.Abp.TenantManagement.Web
{
    [DependsOn(typeof(AbpTenantManagementApplicationContractsModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpFeatureManagementWebModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpTenantManagementWebModule : AbpModule
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpTenantManagementResource), typeof(AbpTenantManagementWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpTenantManagementWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpTenantManagementWebMainMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTenantManagementWebModule>();
            });

            context.Services.AddAutoMapperObjectMapper<AbpTenantManagementWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementWebAutoMapperProfile>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/Index", TenantManagementPermissions.Tenants.Default);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/CreateModal", TenantManagementPermissions.Tenants.Create);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/EditModal", TenantManagementPermissions.Tenants.Update);
            });

            Configure<AbpPageToolbarOptions>(options =>
            {
                options.Configure<Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<AbpTenantManagementResource>("ManageHostFeatures"),
                            icon: "cog",
                            name: "ManageHostFeatures",
                            requiredPolicyName: FeatureManagementPermissions.ManageHostFeatures
                        );

                        toolbar.AddButton(
                            LocalizableString.Create<AbpTenantManagementResource>("NewTenant"),
                            icon: "plus",
                            name: "CreateTenant",
                            requiredPolicyName: TenantManagementPermissions.Tenants.Create
                        );
                    }
                );
            });

            Configure<DynamicJavaScriptProxyOptions>(options =>
            {
                options.DisableModule(TenantManagementRemoteServiceConsts.ModuleName);
            });
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            OneTimeRunner.Run(() =>
            {
                ModuleExtensionConfigurationHelper
                    .ApplyEntityConfigurationToUi(
                        TenantManagementModuleExtensionConsts.ModuleName,
                        TenantManagementModuleExtensionConsts.EntityNames.Tenant,
                        createFormTypes: new[] { typeof(Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants.CreateModalModel.TenantInfoModel) },
                        editFormTypes: new[] { typeof(Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants.EditModalModel.TenantInfoModel) }
                    );
            });
        }
    }
}
