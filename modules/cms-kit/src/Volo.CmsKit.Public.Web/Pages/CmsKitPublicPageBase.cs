using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Public.Web.Pages;

public class CmsKitPublicPageBase : Microsoft.AspNetCore.Mvc.RazorPages.Page
{
    [RazorInject]
    public IStringLocalizer<CmsKitResource> L { get; set; }

    [RazorInject]
    public IPageLayout PageLayout { get; set; }

    public override Task ExecuteAsync()
    {
        return Task.CompletedTask; // Will be overriden by razor pages. (.cshtml)
    }
}
