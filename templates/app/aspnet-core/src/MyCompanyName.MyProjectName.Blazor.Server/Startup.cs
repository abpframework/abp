using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MyCompanyName.MyProjectName.Blazor.Server;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<MyProjectNameBlazorModule>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.InitializeApplication();
    }
}
