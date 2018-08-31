using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing
{
    [DependsOn(
        typeof(AbpSettingsModule),
        typeof(AbpVirtualFileSystemModule)
        )]
    public class AbpEmailingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<EmailSettingProvider>();
            });

            context.Services.AddAssemblyOf<AbpEmailingModule>();
        }
    }
}
