using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp.AspNetCore.TestBase
{
    public abstract class AbpAspNetCoreIntegratedTestBase<TStartup> : AbpTestBaseWithServiceProvider
        where TStartup : class
    {
        protected TestServer Server { get; }

        protected HttpClient Client { get; }

        protected override IServiceProvider ServiceProvider { get; }

        protected AbpAspNetCoreIntegratedTestBase()
        {
            var builder = CreateHostBuilder();

            var host = builder.Build();
            host.Start();

            Server = host.GetTestServer();
            Client = host.GetTestClient();

            ServiceProvider = Server.Services;

            ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
        }

        protected virtual IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                    webBuilder.UseTestServer();
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
}
