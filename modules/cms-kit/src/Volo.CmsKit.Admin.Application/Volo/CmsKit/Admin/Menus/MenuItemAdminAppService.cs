using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.ObjectExtending;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Menus;

[RequiresFeature(CmsKitFeatures.MenuEnable)]
[RequiresGlobalFeature(typeof(MenuFeature))]
[Authorize(CmsKitAdminPermissions.Menus.Default)]
public class MenuItemAdminAppService : CmsKitAdminAppServiceBase, IMenuItemAdminAppService
{
    protected MenuItemManager MenuManager { get; }
    protected IMenuItemRepository MenuItemRepository { get; }
    protected IPageRepository PageRepository { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    public MenuItemAdminAppService(
        MenuItemManager menuManager,
        IMenuItemRepository menuRepository,
        IPageRepository pageRepository, 
        IPermissionDefinitionManager permissionDefinitionManager)
    {
        MenuManager = menuManager;
        MenuItemRepository = menuRepository;
        PageRepository = pageRepository;
        PermissionDefinitionManager = permissionDefinitionManager;
    }

    public virtual async Task<ListResultDto<MenuItemDto>> GetListAsync()
    {
        var menuItems = await MenuItemRepository.GetOrderedListAsync();

        return new ListResultDto<MenuItemDto>(
                ObjectMapper.Map<List<MenuItem>, List<MenuItemDto>>(menuItems)
        );
    }

    public virtual async Task<MenuItemWithDetailsDto> GetAsync(Guid id)
    {
        var menuItem = await MenuItemRepository.GetAsync(id);
        var dto = ObjectMapper.Map<MenuItem, MenuItemWithDetailsDto>(menuItem);

        if (menuItem.PageId.HasValue)
        {
            dto.PageTitle = await PageRepository.FindTitleAsync(menuItem.PageId.Value);
        }

        return dto;
    }

    [Authorize(CmsKitAdminPermissions.Menus.Create)]
    public virtual async Task<MenuItemDto> CreateAsync(MenuItemCreateInput input)
    {
        var menuItem = new MenuItem(
                GuidGenerator.Create(),
                input.DisplayName,
                input.Url.IsNullOrEmpty() ? "#" : input.Url,
                input.IsActive,
                input.ParentId,
                input.Icon,
                input.Order,
                input.Target,
                input.ElementId,
                input.CssClass,
                CurrentTenant.Id,
                input.RequiredPermissionName
            );

        if (input.PageId.HasValue)
        {
            MenuManager.SetPageUrl(menuItem, await PageRepository.GetAsync(input.PageId.Value));
        }
        input.MapExtraPropertiesTo(menuItem);
        await MenuItemRepository.InsertAsync(menuItem);

        return ObjectMapper.Map<MenuItem, MenuItemDto>(menuItem);
    }

    [Authorize(CmsKitAdminPermissions.Menus.Update)]
    public virtual async Task<MenuItemDto> UpdateAsync(Guid id, MenuItemUpdateInput input)
    {
        var menuItem = await MenuItemRepository.GetAsync(id);

        if (input.PageId.HasValue)
        {
            MenuManager.SetPageUrl(menuItem, await PageRepository.GetAsync(input.PageId.Value));
        }
        else
        {
            MenuManager.SetPageUrl(menuItem, input.Url);
        }

        menuItem.SetDisplayName(input.DisplayName);
        menuItem.IsActive = input.IsActive;
        menuItem.Icon = input.Icon;
        menuItem.Target = input.Target;
        menuItem.ElementId = input.ElementId;
        menuItem.CssClass = input.CssClass;
        menuItem.RequiredPermissionName = input.RequiredPermissionName;
        menuItem.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);
        input.MapExtraPropertiesTo(menuItem);
        await MenuItemRepository.UpdateAsync(menuItem);

        return ObjectMapper.Map<MenuItem, MenuItemDto>(menuItem);
    }

    [Authorize(CmsKitAdminPermissions.Menus.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return MenuItemRepository.DeleteAsync(id);
    }

    [Authorize(CmsKitAdminPermissions.Menus.Update)]
    public virtual Task MoveMenuItemAsync(Guid id, MenuItemMoveInput input)
    {
        return MenuManager.MoveAsync(id, input.NewParentId, input.Position);
    }

    public virtual async Task<PagedResultDto<PageLookupDto>> GetPageLookupAsync(PageLookupInputDto input)
    {
        var count = await PageRepository.GetCountAsync(input.Filter);

        var pages = await PageRepository.GetListAsync(
            input.Filter,
            input.Status,
            input.MaxResultCount,
            input.SkipCount,
            input.Sorting
        );

        return new PagedResultDto<PageLookupDto>(
            count,
            ObjectMapper.Map<List<Page>, List<PageLookupDto>>(pages)
        );
    }
    
    public virtual async Task<ListResultDto<PermissionLookupDto>> GetPermissionLookupAsync(PermissionLookupInputDto inputDto)
    {
        var permissions = await PermissionDefinitionManager.GetPermissionsAsync();

        var permissionLookupDtos= permissions
            .WhereIf(!inputDto.Filter.IsNullOrWhiteSpace(), p => p.Name.Contains(inputDto.Filter, StringComparison.OrdinalIgnoreCase))
            .Select(x => new PermissionLookupDto
        {
            Name = x.Name,
            DisplayName = x.DisplayName.Localize(StringLocalizerFactory)
        }).ToList();
        
        return new ListResultDto<PermissionLookupDto>(
            permissionLookupDtos
        );
    }

    public virtual async Task<int> GetAvailableMenuOrderAsync(Guid? parentId = null)
    {
        var highestOrder = await MenuItemRepository.GetHighestMenuOrderAsync(parentId);
        return highestOrder + 1;
    }
}
