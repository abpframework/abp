using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNameWebTestStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<MyProjectNameWebTestModule>(options =>
            {
                options.UseAutofac();
            });

            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}