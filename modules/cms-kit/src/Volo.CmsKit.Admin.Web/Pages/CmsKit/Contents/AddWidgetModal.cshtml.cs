using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Admin.Contents;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Contents;

public class AddWidgetModal : AbpPageModel
{
    protected IContentAdminAppService ContentAdminAppService { get; }

    [BindProperty]
    public ContentViewModel ViewModel { get; set; }

    public List<SelectListItem> Widgets { get; set; } = new();

    public AddWidgetModal(IContentAdminAppService contentAdminAppService)
    {
        ContentAdminAppService = contentAdminAppService;
    }

    public async Task OnGetAsync()
    {
        Widgets = new List<SelectListItem>() { new("", "") };
        Widgets.AddRange((await ContentAdminAppService.GetWidgetsAsync())
            .Items
            .Select(w => new SelectListItem(w.Key, w.Key))
            .ToList());
    }

    public class ContentViewModel
    {
        [SelectItems(nameof(Widgets))]
        public string Widget { get; set; }
    }
}
