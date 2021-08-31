using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public interface IClientProxyApiDescriptionFinder
    {
        Task<ActionApiDescriptionModel> FindActionAsync(string methodName);

        Task<ApplicationApiDescriptionModel> GetApiDescriptionAsync();
    }
}
