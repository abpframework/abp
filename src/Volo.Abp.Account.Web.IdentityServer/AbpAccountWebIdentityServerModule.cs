using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account.Web
{
    [DependsOn(typeof(AbpAccountWebModule))]
    [DependsOn(typeof(AbpIdentityServerDomainModule))]
    public class AbpAccountWebIdentityServerModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountWebIdentityServerModule>("Volo.Abp.Account.Web");
            });

            services.AddAssemblyOf<AbpAccountWebIdentityServerModule>();
        }
    }
}
