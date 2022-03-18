using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web;

public interface IServerUrlProvider
{
    Task<string> GetBaseUrlAsync(string remoteServiceName = null);
}
