using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Configuration;

public interface ICurrentApplicationConfigurationCacheResetService
{
    Task ResetAsync();
}
