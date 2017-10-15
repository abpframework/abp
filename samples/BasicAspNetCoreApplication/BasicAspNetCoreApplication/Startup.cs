using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace BasicAspNetCoreApplication
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AppModule>(options =>
            {
                options.UseAutofac();
            });

            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
