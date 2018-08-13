using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.Modularity;

namespace Volo.Abp.Storage
{
    [DependsOn(typeof(AbpStorageModule))]
    public class AbpFileSystemStorageModule : AbpModule
    {
        private readonly IHostingEnvironment _environment;

        public AbpFileSystemStorageModule(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpStorage(context.Services.GetConfiguration().GetSection("Storage"))
                .AddFileSystemStorage(_environment.ContentRootPath);
            
            context.Services.AddAssemblyOf<AbpFileSystemStorageModule>();
        }
    }
}