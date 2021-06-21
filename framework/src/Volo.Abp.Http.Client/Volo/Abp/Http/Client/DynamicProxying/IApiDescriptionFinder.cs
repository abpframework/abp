using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public interface IApiDescriptionFinder
    {
        Task<ActionApiDescriptionModel> FindActionAsync(HttpClient client, string baseUrl, Type serviceType, MethodInfo invocationMethod);

        Task<ApplicationApiDescriptionModel> GetApiDescriptionAsync(HttpClient client, string baseUrl);
    }
}
