using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.TestBase;

/// <typeparam name="TStartupModule">
/// Can be a module type or old-style ASP.NET Core Startup class.
/// </typeparam>
[Obsolete("Use AbpWebApplicationFactoryIntegratedTest instead.")]
public abstract class AbpAspNetCoreIntegratedTestBase<TStartupModule> : AbpTestBaseWithServiceProvider, IDisposable
    where TStartupModule : class
{
    protected TestServer Server { get; }

    protected HttpClient Client { get; }

    private readonly IHost _host;

    protected AbpAspNetCoreIntegratedTestBase()
    {
        var builder = CreateHostBuilder();

        _host = builder.Build();
        _host.Start();

        Server = _host.GetTestServer();
        Client = _host.GetTestClient();

        ServiceProvider = Server.Services;

        ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
    }

    protected virtual IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .AddAppSettingsSecretsJson()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                if (typeof(TStartupModule).IsAssignableTo<IAbpModule>())
                {
                    webBuilder.UseStartup<TestStartup<TStartupModule>>();
                }
                else
                {
                    webBuilder.UseStartup<TStartupModule>();
                }

                webBuilder.UseAbpTestServer();
            })
            .UseAutofac()
            .ConfigureServices(ConfigureServices);
    }

    protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {

    }

    #region GetUrl

    /// <summary>
    /// Gets default URL for given controller type.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    protected virtual string GetUrl<TController>()
    {
        return "/" + typeof(TController).Name.RemovePostFix("Controller", "AppService", "ApplicationService", "IntService", "IntegrationService", "Service");
    }

    /// <summary>
    /// Gets default URL for given controller type's given action.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    protected virtual string GetUrl<TController>(string actionName)
    {
        return GetUrl<TController>() + "/" + actionName;
    }

    /// <summary>
    /// Gets default URL for given controller type's given action with query string parameters (as anonymous object).
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    protected virtual string GetUrl<TController>(string actionName, object queryStringParamsAsAnonymousObject)
    {
        var url = GetUrl<TController>(actionName);

        var dictionary = new RouteValueDictionary(queryStringParamsAsAnonymousObject);
        if (dictionary.Any())
        {
            url += "?" + dictionary.Select(d => $"{d.Key}={d.Value}").JoinAsString("&");
        }

        return url;
    }

    #endregion

    public void Dispose()
    {
        _host?.Dispose();
    }
}
