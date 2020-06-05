using System;
using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public interface IApiDescriptionCache
    {
        Task<ApplicationApiDescriptionModel> GetAsync(
            string baseUrl,
            Func<Task<ApplicationApiDescriptionModel>> factory
        );
    }
}