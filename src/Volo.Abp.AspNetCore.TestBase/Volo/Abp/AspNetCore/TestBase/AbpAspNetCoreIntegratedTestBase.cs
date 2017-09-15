using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.TestBase;

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
            var builder = CreateWebHostBuilder();
            Server = CreateTestServer(builder);
            Client = Server.CreateClient();
            ServiceProvider = Server.Host.Services;

            ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
        }

        protected virtual IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseStartup<TStartup>();
        }

        protected virtual TestServer CreateTestServer(IWebHostBuilder builder)
        {
            return new TestServer(builder);
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
