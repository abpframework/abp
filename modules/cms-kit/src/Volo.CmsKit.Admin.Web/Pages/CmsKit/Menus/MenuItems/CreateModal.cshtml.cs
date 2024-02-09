using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.ObjectExtending;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems;

public class CreateModalModel : CmsKitAdminPageModel
{
    protected IMenuItemAdminAppService MenuAdminAppService { get; }
    protected IFeatureChecker FeatureChecker { get; }

    [BindProperty]
    public MenuItemCreateViewModel ViewModel { get; set; }

    public bool IsPageFeatureEnabled { get; set; }

    public CreateModalModel(IMenuItemAdminAppService menuAdminAppService, IFeatureChecker featureChecker)
    {
        MenuAdminAppService = menuAdminAppService;
        FeatureChecker = featureChecker;
        ViewModel = new MenuItemCreateViewModel();
    }

    public virtual async Task OnGetAsync(Guid? parentId)
    {
        ViewModel.ParentId = parentId;

        IsPageFeatureEnabled = GlobalFeatureManager.Instance.IsEnabled<PagesFeature>()
            && await FeatureChecker.IsEnabledAsync(CmsKitFeatures.PageEnable);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var input = ObjectMapper.Map<MenuItemCreateViewModel, MenuItemCreateInput>(ViewModel);

        var dto = await MenuAdminAppService.CreateAsync(input);

        return new OkObjectResult(dto);
    }

    [AutoMap(typeof(MenuItemCreateInput), ReverseMap = true)]
    public class MenuItemCreateViewModel : ExtensibleObject
    {
        [HiddenInput]
        public Guid? ParentId { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public string Url { get; set; }

        public Guid? PageId { get; set; }

        public string Icon { get; set; }

        [HiddenInput]
        public int Order { get; set; }

        public string Target { get; set; }

        public string ElementId { get; set; }

        public string CssClass { get; set; }

    }
}
