using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Components.ExceptionHandling;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.AspNetCore.Components.Web.ExceptionHandling
{
    [Dependency(ReplaceServices = true)]
    public class UserExceptionInformer : IUserExceptionInformer, ITransientDependency
    {
        public ILogger<UserExceptionInformer> Logger { get; set; }
        protected IUiMessageService MessageService { get; }
        protected IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; }

        public UserExceptionInformer(
            IUiMessageService messageService,
            IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
        {
            MessageService = messageService;
            ExceptionToErrorInfoConverter = exceptionToErrorInfoConverter;
            Logger = NullLogger<UserExceptionInformer>.Instance;
        }

        public void Inform(UserExceptionInformerContext context)
        {
            //TODO: Create sync versions of the MessageService APIs.

            var errorInfo = GetErrorInfo(context);

            if (errorInfo.Details.IsNullOrEmpty())
            {
                MessageService.Error(errorInfo.Message);
            }
            else
            {
                MessageService.Error(errorInfo.Details, errorInfo.Message);
            }
        }

        public async Task InformAsync(UserExceptionInformerContext context)
        {
            var errorInfo = GetErrorInfo(context);

            if (errorInfo.Details.IsNullOrEmpty())
            {
                await MessageService.Error(errorInfo.Message);
            }
            else
            {
                await MessageService.Error(errorInfo.Details, errorInfo.Message);
            }
        }

        protected virtual RemoteServiceErrorInfo GetErrorInfo(UserExceptionInformerContext context)
        {
            // if (context.Exception is AbpRemoteCallException remoteCallException)
            // {
            //     return remoteCallException.Error;
            // }

            return ExceptionToErrorInfoConverter.Convert(context.Exception, false);
        }
    }
}
