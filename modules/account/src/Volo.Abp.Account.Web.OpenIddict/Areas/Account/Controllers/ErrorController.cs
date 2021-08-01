using AutoMapper.Internal;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error;
using Volo.Abp.Http;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers
{
    [Area("account")]
    public class ErrorController : AbpController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AbpErrorPageOptions _abpErrorPageOptions;

        public ErrorController(
            IWebHostEnvironment environment,
            IOptions<AbpErrorPageOptions> abpErrorPageOptions)
        {
            _environment = environment;
            _abpErrorPageOptions = abpErrorPageOptions.Value;
        }

        public virtual async Task<IActionResult> Index(string errorId)
        {
            await Task.CompletedTask;

            // If the error was not caused by an invalid
            // OIDC request, display a generic error page.
            var response = HttpContext.GetOpenIddictServerResponse();

            var errorMessage = response?.Error ?? L["Error"];
            string errorDescription = null;

            if (_environment.IsDevelopment())
            {
                // Only show in development
                errorDescription = response?.ErrorDescription;
            }

            const int statusCode = (int)HttpStatusCode.InternalServerError;

            return View(GetErrorPageUrl(statusCode), new AbpErrorViewModel
            {
                ErrorInfo = new RemoteServiceErrorInfo(errorMessage, errorDescription),
                HttpStatusCode = statusCode
            });
        }

        protected virtual string GetErrorPageUrl(int statusCode)
        {
            var page = _abpErrorPageOptions.ErrorViewUrls.GetOrDefault(statusCode.ToString());

            return string.IsNullOrWhiteSpace(page) ? "~/Views/Error/Default.cshtml" : page;
        }
    }
}
