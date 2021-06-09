using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Admin.Menus;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems
{
    public class IndexModel : CmsKitAdminPageModel
    {
        protected IMenuAdminAppService MenuAdminAppService { get; }

        public IndexModel(IMenuAdminAppService menuAdminAppService)
        {
            MenuAdminAppService = menuAdminAppService;
        }

        [BindProperty(SupportsGet = true)] 
        public Guid Id { get; set; }

        public MenuDto Menu { get; protected set; }

        public async Task OnGetAsync()
        {
            Menu = await MenuAdminAppService.GetSimpleAsync(Id);
        }
    }
}