using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems
{
    public class IndexModel : CmsKitAdminPageModel
    {
        protected IMenuAdminAppService MenuAdminAppService { get; }

        [BindProperty(SupportsGet = true)] 
        public Guid Id { get; set; }

        public MenuWithDetailsDto Menu { get; protected set; }

        public IndexModel(IMenuAdminAppService menuAdminAppService)
        {
            MenuAdminAppService = menuAdminAppService;
        }

        public async Task OnGetAsync()
        {
            Menu = await MenuAdminAppService.GetAsync(Id);
        }
    }
}