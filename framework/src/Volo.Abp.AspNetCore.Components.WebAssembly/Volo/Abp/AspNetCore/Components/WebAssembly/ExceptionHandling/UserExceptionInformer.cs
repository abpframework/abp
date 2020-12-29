using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Http.Client;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.ExceptionHandling
{
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
            var errorInfo = GetErrorInfo(context);

            if (errorInfo.Details.IsNullOrEmpty())
            {
                //TODO: Should we introduce MessageService.Error (sync) method instead of such a usage (without await)..?
                MessageService.Error(errorInfo.Message);
            }
            else
            {
                MessageService.Error(errorInfo.Details, errorInfo.Message);
            }
        }

        protected virtual RemoteServiceErrorInfo GetErrorInfo(UserExceptionInformerContext context)
        {
            if (context.Exception is AbpRemoteCallException remoteCallException)
            {
                return remoteCallException.Error;
            }

            return ExceptionToErrorInfoConverter.Convert(context.Exception, false);
        }
    }
}
