using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Modularity.PlugIns;

namespace Volo.Abp.AspNetCore.Mvc;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<AbpAspNetCoreMvcTestModule>(options =>
        {
            var hostEnvironment = services.GetHostingEnvironment();
            var currentDirectory = hostEnvironment.ContentRootPath;
            var plugDllInPath = "";

            for (var i = 0; i < 10; i++)
            {
                var parentDirectory = new DirectoryInfo(currentDirectory).Parent;
                if (parentDirectory == null)
                {
                    break;
                }

                if (parentDirectory.Name == "test")
                {
#if DEBUG
                    plugDllInPath = Path.Combine(parentDirectory.FullName, "Volo.Abp.AspNetCore.Mvc.PlugIn", "bin", "Debug", "net7.0");
#else
                    plugDllInPath = Path.Combine(parentDirectory.FullName, "Volo.Abp.AspNetCore.Mvc.PlugIn", "bin", "Release", "net7.0");
#endif
                    break;
                }

                currentDirectory = parentDirectory.FullName;
            }

            if (plugDllInPath.IsNullOrWhiteSpace())
            {
                throw new AbpException("Could not find the plug DLL path!");
            }

            options.PlugInSources.AddFolder(plugDllInPath);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
