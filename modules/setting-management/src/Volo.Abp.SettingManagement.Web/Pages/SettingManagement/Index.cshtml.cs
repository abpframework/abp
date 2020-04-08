using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement
{
    public class IndexModel : AbpPageModel
    {
        public SettingPageCreationContext SettingPageCreationContext { get; private set; }

        protected SettingManagementPageOptions Options { get; }

        public IndexModel(IOptions<SettingManagementPageOptions> options)
        {
            Options = options.Value;
        }

        public virtual async Task OnGetAsync()
        {
            SettingPageCreationContext = new SettingPageCreationContext(ServiceProvider);

            foreach (var contributor in Options.Contributors)
            {
                await contributor.ConfigureAsync(SettingPageCreationContext);
            }
        }

        public virtual Task OnPostAsync()
        {
            return Task.CompletedTask;
        }
    }
}