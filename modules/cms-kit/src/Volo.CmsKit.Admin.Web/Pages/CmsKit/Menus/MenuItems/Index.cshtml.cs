using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems
{
    public class IndexModel : CmsKitAdminPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
    }
}
