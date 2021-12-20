using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.TestBase;

public class AbpAspNetCoreAsyncIntegratedTestBase<TModule>
    where TModule : AbpModule
{
    protected IHost Host { get; set; }

    protected TestServer Server { get; set; }

    protected HttpClient Client { get; set; }

    protected IServiceProvider ServiceProvider { get; set; }

    protected virtual T GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    protected virtual T GetRequiredService<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    public virtual async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Host.ConfigureServices(services =>
            {
                services.AddSingleton<IHostLifetime, TestNoopHostLifetime>();
                services.AddSingleton<IServer, TestServer>();
            })
            .UseAutofac();

        await builder.Services.AddApplicationAsync<TModule>(options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
        });

        await ConfigureServicesAsync(builder.Services);

        var app = builder.Build();

        await app.InitializeApplicationAsync();

        await app.StartAsync();

        Host = app.Services.GetRequiredService<IHost>();

        Server = Host.GetTestServer();
        Client = Host.GetTestClient();

        ServiceProvider = Server.Services;

        ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task ConfigureServicesAsync(IServiceCollection services)
    {
        return Task.CompletedTask;
    }

    #region GetUrl

    /// <summary>
    /// Gets default URL for given controller type.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    protected virtual string GetUrl<TController>()
    {
        return "/" + typeof(TController).Name.RemovePostFix("Controller", "AppService", "ApplicationService", "Service");
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
}
