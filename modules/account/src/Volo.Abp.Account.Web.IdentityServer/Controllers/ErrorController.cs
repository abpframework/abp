using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error;
using Volo.Abp.Http;

namespace Volo.Abp.Account.Web.Controllers
{
    [Area("account")]
    public class ErrorController : AbpController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;

        public ErrorController(
            IIdentityServerInteractionService interaction,
            IWebHostEnvironment environment)
        {
            _interaction = interaction;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string errorId)
        {
            var errorMessage = await _interaction.GetErrorContextAsync(errorId) ?? new ErrorMessage
            {
                Error = L["Error"]
            };

            if (!_environment.IsDevelopment())
            {
                // Only show in development
                errorMessage.ErrorDescription = null;
            }


            // ReSharper disable once Mvc.ViewNotResolved
            return View("~/Views/Error/Default.cshtml", new AbpErrorViewModel
            {
                ErrorInfo = new RemoteServiceErrorInfo(errorMessage.Error, errorMessage.ErrorDescription),
                HttpStatusCode = 500
            });
        }
    }
}
