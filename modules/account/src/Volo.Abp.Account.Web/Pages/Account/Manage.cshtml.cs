using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class ManageModel : AccountPageModel
    {
        public ProfileManagementPageCreationContext ProfileManagementPageCreationContext { get; private set; }

        protected ProfileManagementPageOptions Options { get; }

        public ManageModel(IOptions<ProfileManagementPageOptions> options)
        {
            Options = options.Value;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            ProfileManagementPageCreationContext = new ProfileManagementPageCreationContext(ServiceProvider);

            foreach (var contributor in Options.Contributors)
            {
                await contributor.ConfigureAsync(ProfileManagementPageCreationContext);
            }

            return Page();
        }

        public virtual Task<IActionResult> OnPostAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }
    }
}
