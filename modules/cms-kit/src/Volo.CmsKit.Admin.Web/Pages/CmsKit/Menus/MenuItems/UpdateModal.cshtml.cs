using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.ObjectExtending;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems;

public class UpdateModalModel : CmsKitAdminPageModel
{
    protected IMenuItemAdminAppService MenuAdminAppService { get; }
    protected IFeatureChecker FeatureChecker { get; }

    [BindProperty]
    public MenuItemUpdateViewModel ViewModel { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public bool IsPageFeatureEnabled { get; set; }

    public UpdateModalModel(IMenuItemAdminAppService menuAdminAppService, IFeatureChecker featureChecker)
    {
        MenuAdminAppService = menuAdminAppService;
        FeatureChecker = featureChecker;
        IsPageFeatureEnabled = GlobalFeatureManager.Instance.IsEnabled<PagesFeature>();
    }

    public async Task OnGetAsync()
    {
        var menuItemDto = await MenuAdminAppService.GetAsync(Id);

        IsPageFeatureEnabled = GlobalFeatureManager.Instance.IsEnabled<PagesFeature>()
            && await FeatureChecker.IsEnabledAsync(CmsKitFeatures.PageEnable);

        ViewModel = ObjectMapper.Map<MenuItemWithDetailsDto, MenuItemUpdateViewModel>(menuItemDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var input = ObjectMapper.Map<MenuItemUpdateViewModel, MenuItemUpdateInput>(ViewModel);

        var result = await MenuAdminAppService.UpdateAsync(Id, input);

        return new OkObjectResult(result);
    }

    public class MenuItemUpdateViewModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [Required]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Url { get; set; }

        public string Icon { get; set; }

        public string Target { get; set; }

        public string ElementId { get; set; }

        public string CssClass { get; set; }

        public Guid? PageId { get; set; }

        public string? PageTitle { get; set; }

        [HiddenInput]
        public string ConcurrencyStamp { get; set; }
    }
}
