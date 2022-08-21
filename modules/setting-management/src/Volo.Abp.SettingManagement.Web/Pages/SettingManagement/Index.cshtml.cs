using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Features;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

[Authorize]
[RequiresFeature(SettingManagementFeatures.Enable)]
public class IndexModel : AbpPageModel
{
    public SettingPageCreationContext SettingPageCreationContext { get; private set; }

    protected SettingPageContributorManager SettingPageContributorManager { get; }

    protected ILocalEventBus LocalEventBus { get; }

    public IndexModel(ILocalEventBus localEventBus, SettingPageContributorManager settingPageContributorManager)
    {
        LocalEventBus = localEventBus;
        SettingPageContributorManager = settingPageContributorManager;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        SettingPageCreationContext = await SettingPageContributorManager.ConfigureAsync();

        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual async Task<IActionResult> OnPostRenderViewAsync(string id)
    {
        var context = await SettingPageContributorManager.ConfigureAsync();

        var view = context.Groups.FirstOrDefault(x => x.Id == id);
        if (view != null)
        {
            return ViewComponent(view.ComponentType, view.Parameter);
        }

        return NoContent();
    }

    public virtual async Task<NoContentResult> OnPostRefreshConfigurationAsync()
    {
        await LocalEventBus.PublishAsync(
            new CurrentApplicationConfigurationCacheResetEventData()
        );

        return NoContent();
    }
}
