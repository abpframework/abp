using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.CmsKit.Admin.RobotsTxts;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.RobotsTxt;

public class IndexModel : CmsKitAdminPageModel
{
    protected IRobotsTxtAdminAppService RobotsTxtAdminAppService { get; }

    [TextArea(Rows = 20)]
    public string Content { get; set; }

    public RobotsTxtSource Source { get; set; }

    public IndexModel(IRobotsTxtAdminAppService robotsTxtAdminAppService)
    {
        RobotsTxtAdminAppService = robotsTxtAdminAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var robotsTxt = await RobotsTxtAdminAppService.GetAsync();

        Source = robotsTxt.Source;
        Content = robotsTxt.Content;
    }
}
