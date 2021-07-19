using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Features;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement
{
    [RequiresFeature(SettingManagementFeatures.Enable)]
    public class IndexModel : AbpPageModel
    {
        public SettingPageCreationContext SettingPageCreationContext { get; private set; }

        protected ILocalEventBus LocalEventBus { get; }
        protected SettingManagementPageOptions Options { get; }

        public IndexModel(
            IOptions<SettingManagementPageOptions> options,
            ILocalEventBus localEventBus)
        {
            LocalEventBus = localEventBus;
            Options = options.Value;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            SettingPageCreationContext = new SettingPageCreationContext(ServiceProvider);

            foreach (var contributor in Options.Contributors)
            {
                await contributor.ConfigureAsync(SettingPageCreationContext);
            }

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
}
