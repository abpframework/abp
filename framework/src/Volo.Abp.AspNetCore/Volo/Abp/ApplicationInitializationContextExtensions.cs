using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp;

public static class ApplicationInitializationContextExtensions
{
    public static IApplicationBuilder GetApplicationBuilder(this ApplicationInitializationContext context)
    {
        var applicationBuilder = context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
        Check.NotNull(applicationBuilder, nameof(applicationBuilder));
        return applicationBuilder;
    }

    public static IApplicationBuilder? GetApplicationBuilderOrNull(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
    }

    public static IHost GetHost(this ApplicationInitializationContext context)
    {
        var host = context.ServiceProvider.GetRequiredService<IObjectAccessor<IHost>>().Value;
        Check.NotNull(host, nameof(host));
        return host;
    }

    public static IHost? GetHostOrNull(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IObjectAccessor<IHost>>().Value;
    }

    public static IEndpointRouteBuilder GetEndpointRouteBuilder(this ApplicationInitializationContext context)
    {
        var endpointRouteBuilder = context.ServiceProvider.GetRequiredService<IObjectAccessor<IEndpointRouteBuilder>>().Value;
        Check.NotNull(endpointRouteBuilder, nameof(endpointRouteBuilder));
        return endpointRouteBuilder;
    }

    public static IEndpointRouteBuilder? GetEndpointRouteBuilderOrNull(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IObjectAccessor<IEndpointRouteBuilder>>().Value;
    }

    public static WebApplication GetWebApplication(this ApplicationInitializationContext context)
    {
        var webApplication = context.ServiceProvider.GetRequiredService<IObjectAccessor<WebApplication>>().Value;
        Check.NotNull(webApplication, nameof(webApplication));
        return webApplication;
    }

    public static WebApplication? GetWebApplicationOrNull(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IObjectAccessor<WebApplication>>().Value;
    }

    public static IWebHostEnvironment GetEnvironment(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    }

    public static IWebHostEnvironment? GetEnvironmentOrNull(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetService<IWebHostEnvironment>();
    }

    public static IConfiguration GetConfiguration(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    public static ILoggerFactory GetLoggerFactory(this ApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<ILoggerFactory>();
    }
}
