using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus
{
    public class UpdateModalModel : CmsKitAdminPageModel
    {
        protected IMenuAdminAppService MenuAdminAppService { get; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public UpdateMenuViewModel ViewModel { get; set; }

        public UpdateModalModel(IMenuAdminAppService menuAdminAppService)
        {
            MenuAdminAppService = menuAdminAppService;
        }

        public async Task OnGetAsync()
        {
            var menu = await MenuAdminAppService.GetAsync(Id);

            ViewModel = ObjectMapper.Map<MenuWithDetailsDto, UpdateMenuViewModel>(menu);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<UpdateMenuViewModel, MenuUpdateInput>(ViewModel);

            await MenuAdminAppService.UpdateAsync(Id, input);

            return NoContent();
        }

        [AutoMap(typeof(MenuWithDetailsDto))]
        [AutoMap(typeof(MenuUpdateInput), ReverseMap = true)]
        public class UpdateMenuViewModel
        {
            [Required]
            [DynamicMaxLength(typeof(MenuConsts), nameof(MenuConsts.MaxNameLength))]
            public string Name { get; set; }
        }
    }
}
