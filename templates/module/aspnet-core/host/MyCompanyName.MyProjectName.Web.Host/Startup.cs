using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyCompanyName.MyProjectName;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<MyProjectNameWebHostModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
