using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Volo.Abp;
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

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AbpDeskWebMvcModule>(options =>
            {
                options.UseAutofac();

                /* @halil: I added Abp.MongoDb package as a dependency to the main application in order to use the blog plugin.
                 * Otherwise, we should add all dependencies (Recursively) into plugin folder
                 * and load in correct order. We should carefully think on that problem in the future.
                 * NOTE: Disabled 
                 */
                //options.PlugInSources.AddFolder(
                //    Path.Combine(
                //        _env.ContentRootPath,
                //        @"../Web_PlugIns/")
                //);
            });


            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "911417875702990";
                facebookOptions.AppSecret = "adea0bff222ae340d8fb0ce3e6275d6b";
            });



            //TODO: This is needed because ASP.NET Core does not use IServiceProviderFactory!
            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory
                .AddConsole()
                .AddDebug()
                .AddSerilog(new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.RollingFile("Logs/logs.txt")
                    .CreateLogger()
                );

            app.InitializeApplication();
        }
    }
}
