using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account.Web
{
    [DependsOn(typeof(AbpAccountWebModule))]
    public class AbpAccountWebIdentityServerModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAccountWebModule>();

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountWebModule>("Volo.Abp.Account.Web");
            });
        }
    }
}
