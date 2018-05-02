using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Http;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Pages.Error
{
    public class IndexModel : PageModel
    {
        public RemoteServiceErrorInfo ErrorInfo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int HttpStatusCode { get; set; }

        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;

        public IndexModel(IExceptionToErrorInfoConverter errorInfoConverter)
        {
            _errorInfoConverter = errorInfoConverter;
        }

        public void OnGet()
        {
            var exHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var exception = exHandlerFeature != null
                ? exHandlerFeature.Error
                : new Exception("Unhandled exception!"); //TODO: Localize?

            ErrorInfo = _errorInfoConverter.Convert(exception);

            if (HttpStatusCode == 0)
            {
                HttpStatusCode = HttpContext.Response.StatusCode;
            }
        }
    }
}