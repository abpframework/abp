using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Admin.Menus;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems
{
    public class CreateModalModel : CmsKitAdminPageModel
    {
        protected IMenuAdminAppService MenuAdminAppService { get; }

        [BindProperty]
        public MenuItemCreateViewModel ViewModel { get; set; }

        public CreateModalModel(IMenuAdminAppService menuAdminAppService)
        {
            MenuAdminAppService = menuAdminAppService;
            ViewModel = new MenuItemCreateViewModel();
        }

        public virtual Task OnGetAsync(Guid menuId, Guid? parentId)
        {
            ViewModel.MenuId = menuId;
            ViewModel.ParentId = parentId;

            return Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<MenuItemCreateViewModel, MenuItemCreateInput>(ViewModel);

            var dto = await MenuAdminAppService.CreateMenuItemAsync(ViewModel.MenuId, input);

            return new OkObjectResult(dto);
        }

        [AutoMap(typeof(MenuItemCreateInput), ReverseMap = true)]
        public class MenuItemCreateViewModel
        {
            [HiddenInput]
            public Guid MenuId { get; set; }

            [HiddenInput]
            public Guid? ParentId { get; set; }

            [Required]
            public string DisplayName { get; set; }

            public bool IsActive { get; set; }

            [Required]
            public string Url { get; set; }

            public Guid? PageId { get; set; }

            public string Icon { get; set; }

            public int Order { get; set; }

            public string Target { get; set; }

            public string ElementId { get; set; }

            public string CssClass { get; set; }

            public string RequiredPermissionName { get; set; }

        }
    }
}
