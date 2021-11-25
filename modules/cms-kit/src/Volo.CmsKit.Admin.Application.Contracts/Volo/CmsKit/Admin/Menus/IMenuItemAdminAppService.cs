using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Admin.Menus;

public interface IMenuItemAdminAppService : IApplicationService
{
    Task<ListResultDto<MenuItemDto>> GetListAsync();

    Task<MenuItemDto> GetAsync(Guid id);

    Task<MenuItemDto> CreateAsync(MenuItemCreateInput input);

    Task<MenuItemDto> UpdateAsync(Guid id, MenuItemUpdateInput input);

    Task DeleteAsync(Guid id);

    Task MoveMenuItemAsync(Guid id, MenuItemMoveInput input);

    Task<PagedResultDto<PageLookupDto>> GetPageLookupAsync(PageLookupInputDto input);
}
