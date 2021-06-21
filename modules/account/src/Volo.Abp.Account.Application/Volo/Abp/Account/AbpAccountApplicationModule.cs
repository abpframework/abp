using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpUiNavigationModule),
        typeof(AbpEmailingModule)
    )]
    public class AbpAccountApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountApplicationModule>();
            });

            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].Urls[AccountUrlNames.PasswordReset] = "Account/ResetPassword";
            });
        }
    }
}
