using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyModuleName.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(typeof(MyModuleNameHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class MyModuleNameWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(MyModuleNameResource), typeof(MyModuleNameWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new MyModuleNameMenuContributor());
            });

            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyModuleNameWebModule>("MyCompanyName.MyModuleName");
            });

            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MyModuleNameResource>()
                    .AddBaseTypes(
                        typeof(AbpValidationResource),
                        typeof(AbpUiResource)
                    ).AddVirtualJson("/Localization/Resources/MyModuleName");
            });

            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MyModuleNameWebAutoMapperProfile>(validate: true);
            });

            context.Services.Configure<RazorPagesOptions>(options =>
            {
                //Configure authorization.
            });

            context.Services.AddAssemblyOf<MyModuleNameWebModule>();
        }
    }
}
