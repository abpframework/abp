using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class AbpExceptionFilter : IExceptionFilter, ITransientDependency
    {
        public ILogger<AbpExceptionFilter> Logger { get; set; }

        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
        private readonly HttpExceptionStatusCodeFinder _statusCodeFinder;

        public AbpExceptionFilter(IExceptionToErrorInfoConverter errorInfoConverter, HttpExceptionStatusCodeFinder statusCodeFinder)
        {
            _errorInfoConverter = errorInfoConverter;
            _statusCodeFinder = statusCodeFinder;

            Logger = NullLogger<AbpExceptionFilter>.Instance;
        }

        public void OnException(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                return;
            }

            Logger.LogException(context.Exception);

            HandleAndWrapException(context);
        }

        private bool ShouldHandleException(ExceptionContext context)
        {
            if (context.ActionDescriptor.IsControllerAction() &&
                ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
            {
                //TODO: Create DontWrap attribute to control wrapping..?

                return true;
            }

            var accept = context.HttpContext.Request.Headers["Accept"];
            if (accept.ToString().Contains("application/json")) //TODO: Optimize
            {
                return true;
            }

            return false;
        }

        private void HandleAndWrapException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = _statusCodeFinder.GetStatusCode(context.HttpContext, context.Exception);
            context.HttpContext.Response.Headers.Add(new KeyValuePair<string, StringValues>("_AbpErrorFormat", "true"));

            context.Result = new ObjectResult(
                new RemoteServiceErrorResponse(
                    _errorInfoConverter.Convert(context.Exception)
                )
            );

            //TODO: Trigger an AbpExceptionHandled event or something like that.

            context.Exception = null; //Handled!
        }
    }
}
