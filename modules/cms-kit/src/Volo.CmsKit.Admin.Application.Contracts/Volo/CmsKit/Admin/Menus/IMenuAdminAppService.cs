using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Admin.Menus
{
    public interface IMenuAdminAppService : IApplicationService
    {
        Task<PagedResultDto<MenuDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<MenuWithDetailsDto> GetAsync(Guid id);

        Task<MenuDto> GetSimpleAsync(Guid id);

        Task<MenuDto> CreateAsync(MenuCreateInput input);

        Task<MenuDto> UpdateAsync(Guid menuId, MenuUpdateInput input);

        Task DeleteAsync(Guid menuId);

        Task<MenuItemDto> GetMenuItemAsync(Guid menuId, Guid menuItemId);
        
        Task<MenuItemDto> CreateMenuItemAsync(Guid menuId, MenuItemCreateInput input);

        Task<MenuItemDto> UpdateMenuItemAsync(Guid menuId, Guid menuItemId, MenuItemUpdateInput input);

        Task DeleteMenuItemAsync(Guid menuId, Guid menuItemId);

        Task MoveMenuItemAsync(Guid menuId, Guid menuItemId, MenuItemMoveInput input);

        Task UpdateMainMenuAsync(Guid menuId, UpdateMainMenuInput input);
    }
}
