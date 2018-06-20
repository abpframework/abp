using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Localization.MyProjectName;
using MyCompanyName.MyProjectName.Settings;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(typeof(AbpIdentityDomainModule))]
    public class MyProjectNameDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameDomainModule>();
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MyProjectNameResource>("en")
                    .AddVirtualJson("/Localization/MyProjectName");
            });

            services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<MyProjectNameSettingDefinitionProvider>();
            });

            services.AddAssemblyOf<MyProjectNameDomainModule>();
        }
    }
}
