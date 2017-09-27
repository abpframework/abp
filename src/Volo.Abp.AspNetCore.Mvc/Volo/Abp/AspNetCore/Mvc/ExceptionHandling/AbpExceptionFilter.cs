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

        //TODO: Use EventBus to trigger error handled event like in previous ABP

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
            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            //TODO: Create DontWrap attribute to control wrapping..?

            Logger.LogException(context.Exception);
            HandleAndWrapException(context);
        }

        private void HandleAndWrapException(ExceptionContext context)
        {
            if (!context.ActionDescriptor.IsControllerAction() || !ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = _statusCodeFinder.GetStatusCode(context.HttpContext, context.Exception);
            context.HttpContext.Response.Headers.Add(new KeyValuePair<string, StringValues>("_AbpErrorFormat", "true"));

            context.Result = new ObjectResult(
                new RemoteServiceErrorResponse(
                    _errorInfoConverter.Convert(context.Exception)
                )
            );

            //EventBus.Trigger(this, new AbpHandledExceptionData(context.Exception));

            context.Exception = null; //Handled!
        }
    }
}
