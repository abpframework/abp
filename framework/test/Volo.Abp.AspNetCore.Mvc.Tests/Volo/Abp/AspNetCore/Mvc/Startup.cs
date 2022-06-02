using System.IO;
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
#if DEBUG
            var plugDllInPath = Path.Combine(hostEnvironment.ContentRootPath,
                @"..\..\..\..\..\Volo.Abp.AspNetCore.Mvc.PlugIn\bin\Debug\net6.0\");
#else
            plugDllInPath = Path.Combine(_env.ContentRootPath,
                @"..\..\..\..\..\Volo.Abp.AspNetCore.Mvc.PlugIn\bin\Release\net6.0\");
#endif

            options.PlugInSources.AddFolder(plugDllInPath);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
