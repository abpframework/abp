using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.AspNetCore.Builder
{
    public class AspNetConfigurationContext
    {
        public IApplicationBuilder App { get; }

        public IHostingEnvironment Environment { get; }

        public ILoggerFactory LoggerFactory { get; }

        public AspNetConfigurationContext(IApplicationBuilder app)
        {
            App = app;
            Environment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
            LoggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
        }
    }
}