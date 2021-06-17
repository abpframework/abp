using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Public.Menus
{
    public interface IMenuPublicAppService : IApplicationService
    {
        Task<MenuWithDetailsDto> GetMainMenuAsync();
    }
}