using System;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public interface IApiDescriptionFinder
    {
        Task<ActionApiDescriptionModel> FindActionAsync(string baseUrl, Type serviceType, MethodInfo invocationMethod);

        Task<ApplicationApiDescriptionModel> GetApiDescriptionAsync(string baseUrl);
    }
}