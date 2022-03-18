using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace MyCompanyName.MyProjectName;

public class MyProjectNameWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<MyProjectNameWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
