using System;
using Volo.Abp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ProductService.Host
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<ProductServiceHostModule>(options =>
            {
                options.UseAutofac();
                options.Configuration.UserSecretsAssembly = typeof(Startup).Assembly;
            });

            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           app.InitializeApplication();
        }
    }
}
