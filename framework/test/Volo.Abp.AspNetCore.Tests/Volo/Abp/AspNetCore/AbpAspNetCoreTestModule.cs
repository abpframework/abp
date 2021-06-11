using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpAspNetCoreModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpAspNetCoreTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreTestModule>();
                //options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreTestModule>(FindProjectPath(hostingEnvironment));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseStaticFiles();
        }

        private string FindProjectPath(IWebHostEnvironment hostEnvironment)
        {
            var directory = new DirectoryInfo(hostEnvironment.ContentRootPath);

            while (directory != null && directory.Name != "Volo.Abp.AspNetCore.Tests")
            {
                directory = directory.Parent;
            }

            return directory?.FullName
                   ?? throw new Exception("Could not find the project path by beginning from " + hostEnvironment.ContentRootPath + ", going through to parents and looking for Volo.Abp.AspNetCore.Tests");
        }
    }
}
