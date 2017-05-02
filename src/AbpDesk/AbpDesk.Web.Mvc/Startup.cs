using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Volo.Abp.Modularity.PlugIns;

namespace AbpDesk.Web.Mvc
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AbpDeskWebMvcModule>(options =>
            {
                /* @halil: I added Abp.MongoDb package as a dependency to the main application in order to use the blog plugin.
                 * Otherwise, we should add all dependencies (Recursively) into plugin folder
                 * and load in correct order. We should carefully think on that problem in the future.
                 */
                options.PlugInSources.AddFolder(
                    Path.Combine(
                        _env.ContentRootPath,
                        @"../Web_PlugIns/")
                );
            });

            var builder = new ContainerBuilder();
            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
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

            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());

            app.InitializeApplication();
        }
    }
}
