using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Modularity.PlugIns;

namespace AbpDesk.Web.Mvc
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AbpDeskWebMvcModule>(options =>
            {
                /* @halil: I added Abp.MongoDb as a dependency to the main application.
                 * Otherwise, we should add all dependencies (Recursively) into plugin folder
                 * and load in correct order. We should carefully think on that problem in the future.
                 */
                options.PlugInSources.AddFolder(
                    Path.Combine(
                        _env.ContentRootPath,
                        @"../Web_PlugIns/")
                );
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}
