using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp.AspNetCore.TestBase;

public abstract class AbpWebApplicationFactoryIntegratedTest<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected HttpClient Client { get; set; }

    protected IServiceProvider ServiceProvider => Services;

    protected AbpWebApplicationFactoryIntegratedTest()
    {
        Client = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder
            .AddAppSettingsSecretsJson()
            .ConfigureServices(ConfigureServices);
        return base.CreateHost(builder);
    }

    protected virtual T? GetService<T>()
    {
        return Services.GetService<T>();
    }

    protected virtual T GetRequiredService<T>() where T : notnull
    {
        return Services.GetRequiredService<T>();
    }

    protected virtual T? GetKeyedServices<T>(object? serviceKey)
    {
        return ServiceProvider.GetKeyedService<T>(serviceKey);
    }

    protected virtual T GetRequiredKeyedService<T>(object? serviceKey) where T : notnull
    {
        return ServiceProvider.GetRequiredKeyedService<T>(serviceKey);
    }

    protected virtual void ConfigureServices(IServiceCollection services)
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
}
