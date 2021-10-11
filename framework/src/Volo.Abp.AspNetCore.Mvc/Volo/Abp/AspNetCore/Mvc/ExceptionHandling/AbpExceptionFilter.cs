using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class AbpExceptionFilter : IAsyncExceptionFilter, ITransientDependency
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                return;
            }

            await HandleAndWrapException(context);
        }

        protected virtual bool ShouldHandleException(ExceptionContext context)
        {
            //TODO: Create DontWrap attribute to control wrapping..?

            if (context.ActionDescriptor.IsControllerAction() &&
                context.ActionDescriptor.HasObjectResult())
            {
                return true;
            }

            if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
            {
                return true;
            }

            if (context.HttpContext.Request.IsAjax())
            {
                return true;
            }

            return false;
        }

        protected virtual async Task HandleAndWrapException(ExceptionContext context)
        {
            //TODO: Trigger an AbpExceptionHandled event or something like that.

            var exceptionHandlingOptions = context.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;
            var exceptionToErrorInfoConverter = context.GetRequiredService<IExceptionToErrorInfoConverter>();
            var remoteServiceErrorInfo  = exceptionToErrorInfoConverter.Convert(context.Exception, exceptionHandlingOptions.SendExceptionsDetailsToClients);

            var logLevel = context.Exception.GetLogLevel();

            var remoteServiceErrorInfoBuilder = new StringBuilder();
            remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
            remoteServiceErrorInfoBuilder.AppendLine(context.GetRequiredService<IJsonSerializer>().Serialize(remoteServiceErrorInfo, indented: true));

            var logger = context.GetService<ILogger<AbpExceptionFilter>>(NullLogger<AbpExceptionFilter>.Instance);

            logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());

            logger.LogException(context.Exception, logLevel);

            await context.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(context.Exception));

            if (context.Exception is AbpAuthorizationException)
            {
                await context.HttpContext.RequestServices.GetRequiredService<IAbpAuthorizationExceptionHandler>()
                    .HandleAsync(context.Exception.As<AbpAuthorizationException>(), context.HttpContext);
            }
            else
            {
                context.HttpContext.Response.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
                context.HttpContext.Response.StatusCode = (int) context
                    .GetRequiredService<IHttpExceptionStatusCodeFinder>()
                    .GetStatusCode(context.HttpContext, context.Exception);

                context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));
            }

            context.Exception = null; //Handled!
        }
    }
}
