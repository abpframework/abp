using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Public.MarkedItems;
using Volo.CmsKit.Web;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.MarkedItemToggle;

[ViewComponent(Name = "CmsMarkedItemToggle")]
[Widget(
    ScriptTypes = new[] { typeof(MarkedItemToggleScriptBundleContributor) },
    StyleTypes = new[] { typeof(MarkedItemToggleStyleBundleContributor) },
    RefreshUrl = "/CmsKitPublicWidgets/MarkedItem",
    AutoInitialize = true
)]
public class MarkedItemToggleViewComponent : AbpViewComponent
{
    protected IMarkedItemPublicAppService MarkedItemPublicAppService { get; set; }

    protected CmsKitUiOptions Options { get; }


    public MarkedItemToggleViewComponent(
        IMarkedItemPublicAppService markedItemPublicAppService,
        IOptions<CmsKitUiOptions> options)
    {
        MarkedItemPublicAppService = markedItemPublicAppService;
        Options = options.Value;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(
        string entityType,
        string entityId,
        bool? needsConfirmation = false)
    {
        var result = await MarkedItemPublicAppService.GetForUserAsync(entityType, entityId);
        var returnUrl = HttpContext.Request.Path.ToString();

        var viewModel = new MarkedItemToggleViewModel
        {
            EntityType = entityType,
            EntityId = entityId,
            NeedsConfirmation = needsConfirmation.GetValueOrDefault(),
            MarkedItem = new MarkedItemViewModel()
            {
                Icon = Options.MarkedItemIcons.GetLocalizedIcon(result.MarkedItem.IconName),
                IsMarkedByCurrentUser = result.IsMarkedByCurrentUser
            },
            ReturnUrl = returnUrl
        };

        return View("~/Pages/CmsKit/Shared/Components/MarkedItemToggle/Default.cshtml", viewModel);
    }
    public class MarkedItemToggleViewModel 
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public bool NeedsConfirmation { get; set; }

        public MarkedItemViewModel MarkedItem { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class MarkedItemViewModel
    {
        [NotNull]
        public string Icon { get; set; }

        public bool IsMarkedByCurrentUser { get; set; }
    }
}
