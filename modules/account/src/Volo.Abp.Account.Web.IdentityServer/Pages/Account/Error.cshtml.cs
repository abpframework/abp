using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class ErrorModel : AbpPageModel
    {
        public ErrorMessage ErrorMessage { get; set; }

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;

        public ErrorModel(IIdentityServerInteractionService interaction, IWebHostEnvironment environment)
        {
            _interaction = interaction;
            _environment = environment;
        }

        public async Task OnGet(string errorId)
        {
            ErrorMessage = await _interaction.GetErrorContextAsync(errorId) ?? new ErrorMessage
            {
                Error = L["Error"]
            };

            if (ErrorMessage != null)
            {
                if (!_environment.IsDevelopment())
                {
                    // Only show in development
                    ErrorMessage.ErrorDescription = null;
                }
            }
        }
    }
}
