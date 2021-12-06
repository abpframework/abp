using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpIdentityAspNetCoreTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<AbpIdentityAspNetCoreTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
