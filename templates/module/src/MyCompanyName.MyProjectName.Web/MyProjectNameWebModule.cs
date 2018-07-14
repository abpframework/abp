using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(typeof(MyProjectNameHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
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
            context.Services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new MyProjectNameMenuContributor());
            });

            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameWebModule>("MyCompanyName.MyProjectName");
            });

            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MyProjectNameResource>()
                    .AddBaseTypes(
                        typeof(AbpValidationResource),
                        typeof(AbpUiResource)
                    ).AddVirtualJson("/Localization/Resources/MyProjectName");
            });

            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MyProjectNameWebAutoMapperProfile>(validate: true);
            });

            context.Services.Configure<RazorPagesOptions>(options =>
            {
                //Configure authorization.
            });

            context.Services.AddAssemblyOf<MyProjectNameWebModule>();
        }
    }
}
