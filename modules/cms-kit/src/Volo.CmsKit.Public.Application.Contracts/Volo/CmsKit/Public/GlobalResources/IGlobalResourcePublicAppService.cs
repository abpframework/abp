using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.GlobalResources;

public interface IGlobalResourcePublicAppService : IApplicationService
{
    Task<GlobalResourceDto> GetGlobalScriptAsync();
    
    Task<GlobalResourceDto> GetGlobalStyleAsync();
}