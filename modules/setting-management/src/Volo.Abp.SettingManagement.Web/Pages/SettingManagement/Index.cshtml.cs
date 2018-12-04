using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement
{
    public class IndexModel : AbpPageModel
    {
        public SettingPageCreationContext SettingPageCreationContext { get; private set; }

        private readonly SettingManagementPageOptions _options;

        public IndexModel(IOptions<SettingManagementPageOptions> options)
        {
            _options = options.Value;
        }

        public async Task OnGetAsync()
        {
            SettingPageCreationContext = new SettingPageCreationContext();

            foreach (var contributor in _options.Contributors)
            {
                await contributor.ConfigureAsync(SettingPageCreationContext);
            }
        }
    }
}