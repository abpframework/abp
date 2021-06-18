using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus
{
    public class CreateModalModel : CmsKitAdminPageModel
    {
        protected IMenuAdminAppService MenuAdminAppService { get; }

        [BindProperty]
        public CreateMenuViewModel ViewModel { get; set; }

        public CreateModalModel(IMenuAdminAppService menuAdminAppService)
        {
            MenuAdminAppService = menuAdminAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var input = ObjectMapper.Map<CreateMenuViewModel, MenuCreateInput>(ViewModel);

            await MenuAdminAppService.CreateAsync(input);

            return NoContent();
        }

        [AutoMap(typeof(MenuCreateInput), ReverseMap = true)]
        public class CreateMenuViewModel
        {
            [Required]
            [DynamicMaxLength(typeof(MenuConsts), nameof(MenuConsts.MaxNameLength))]
            public string Name { get; set; }
        }
    }
}
