using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Http;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Error
{
    public class IndexModel : PageModel
    {
        public RemoteServiceErrorInfo ErrorInfo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int HttpStatusCode { get; set; }

        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
        private readonly IHttpExceptionStatusCodeFinder _statusCodeFinder;

        public IndexModel(IExceptionToErrorInfoConverter errorInfoConverter, IHttpExceptionStatusCodeFinder statusCodeFinder)
        {
            _errorInfoConverter = errorInfoConverter;
            _statusCodeFinder = statusCodeFinder;
        }

        public void OnGet()
        {
            HandleError();
        }

        public void OnPost()
        {
            HandleError();
        }

        public void OnPut()
        {
            HandleError();
        }

        public void OnDelete()
        {
            HandleError();
        }

        public void OnPatch()
        {
            HandleError();
        }

        private void HandleError()
        {
            var exHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var exception = exHandlerFeature != null
                ? exHandlerFeature.Error
                : new Exception("Unhandled exception!"); //TODO: Localize?

            ErrorInfo = _errorInfoConverter.Convert(exception);

            if (HttpStatusCode == 0)
            {
                HttpStatusCode = (int)_statusCodeFinder.GetStatusCode(HttpContext, exception);
            }

            HttpContext.Response.StatusCode = HttpStatusCode;
        }
    }
}