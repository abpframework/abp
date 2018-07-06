using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore
{
    [DependsOn(typeof(AbpThreadingModule))]
    [DependsOn(typeof(AbpSecurityModule))]
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    public class AbpAspNetCoreModule : IAbpModule
    {
        public void ConfigureServices(ServiceConfigurationContext context)
        {
            AddAspNetServices(context.Services);

            context.Services.AddObjectAccessor<IApplicationBuilder>();

            context.Services.AddConfiguration();

            context.Services.AddAssemblyOf<AbpAspNetCoreModule>();
        }

        private static void AddAspNetServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }
    }
}
