using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp.AspNetCore.TestBase;

public abstract class AbpAspNetCoreWebApplicationFactoryIntegratedTestBase<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected HttpClient Client { get; set; }

    public IServiceProvider ServiceProvider => Services;

    protected AbpAspNetCoreWebApplicationFactoryIntegratedTestBase()
    {
        Client = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(ConfigureServices);
        return base.CreateHost(builder);
    }

    public virtual T? GetService<T>()
    {
        return ServiceProvider!.GetService<T>();
    }

    public virtual T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider!.GetRequiredService<T>();
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
