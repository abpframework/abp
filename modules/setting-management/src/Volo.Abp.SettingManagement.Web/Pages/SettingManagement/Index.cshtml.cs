using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Features;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

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

    public virtual async Task<NoContentResult> OnPostRefreshConfigurationAsync()
    {
        await LocalEventBus.PublishAsync(
            new CurrentApplicationConfigurationCacheResetEventData()
        );

        return NoContent();
    }
}
