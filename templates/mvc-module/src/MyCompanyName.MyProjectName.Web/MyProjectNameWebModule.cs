using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(typeof(MyProjectNameHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiThemeSharedModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class MyProjectNameWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(MyProjectNameResource), typeof(MyProjectNameWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new MyProjectNameMenuContributor());
            });

            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameWebModule>("MyCompanyName.MyProjectName");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MyProjectNameResource>()
                    .AddBaseTypes(typeof(AbpUiResource))
                    .AddVirtualJson("/Localization/MyProjectName/Web");
            });

            Configure<AbpAutoMapperOptions>(options =>
            {
                /* Using `true` for the `validate` parameter to
                 * validate the profile on application startup.
                 * See http://docs.automapper.org/en/stable/Configuration-validation.html for more info
                 * about the configuration validation. */
                options.AddProfile<MyProjectNameWebAutoMapperProfile>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                //Configure authorization.
            });
        }
    }
}
