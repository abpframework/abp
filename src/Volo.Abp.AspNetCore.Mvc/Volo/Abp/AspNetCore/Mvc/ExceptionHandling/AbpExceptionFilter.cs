using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Http;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class AbpExceptionFilter : IExceptionFilter, ITransientDependency
    {
        public ILogger<AbpExceptionFilter> Logger { get; set; }

        //TODO: Use EventBus to trigger error handled event like in previous ABP

        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;

        public AbpExceptionFilter(IExceptionToErrorInfoConverter errorInfoConverter)
        {
            _errorInfoConverter = errorInfoConverter;

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
            if (!ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = GetStatusCode(context);

            context.Result = new ObjectResult(
                new RemoteServiceErrorResponse(
                    _errorInfoConverter.Convert(context.Exception)
                )
            );

            //EventBus.Trigger(this, new AbpHandledExceptionData(context.Exception));

            context.Exception = null; //Handled!
        }

        private int GetStatusCode(ExceptionContext context)
        {
            if (context.Exception is AbpAuthorizationException)
            {
                return context.HttpContext.User.Identity.IsAuthenticated
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;
            }

            if (context.Exception is AbpValidationException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            if (context.Exception is EntityNotFoundException)
            {
                return (int)HttpStatusCode.NotFound;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}
