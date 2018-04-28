using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class DefaultHttpExceptionStatusCodeFinder : IHttpExceptionStatusCodeFinder, ITransientDependency
    {
        public virtual HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
        {
            //TODO: If the exception has error code than we can determine the exception from it!

            if (exception is AbpAuthorizationException)
            {
                return httpContext.User.Identity.IsAuthenticated
                    ? HttpStatusCode.Forbidden
                    : HttpStatusCode.Unauthorized;
            }

            if (exception is AbpValidationException)
            {
                return HttpStatusCode.BadRequest;
            }

            if (exception is EntityNotFoundException)
            {
                return HttpStatusCode.NotFound;
            }

            if (exception is IBusinessException)
            {
                return HttpStatusCode.Forbidden;
            }

            return HttpStatusCode.InternalServerError;
        }
    }
}