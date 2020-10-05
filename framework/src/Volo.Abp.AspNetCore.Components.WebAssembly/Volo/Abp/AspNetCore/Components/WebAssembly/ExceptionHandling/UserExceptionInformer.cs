using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Http.Client;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.ExceptionHandling
{
    public class UserExceptionInformer : IUserExceptionInformer, ITransientDependency
    {
        protected IUiMessageService MessageService { get; }
        protected IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; }

        public UserExceptionInformer(IUiMessageService messageService, IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
        {
            MessageService = messageService;
            ExceptionToErrorInfoConverter = exceptionToErrorInfoConverter;
        }

        public virtual async Task InformAsync(UserExceptionInformerContext context)
        {
            var errorInfo = GetErrorInfo(context);
            await ShowErrorInfoAsync(errorInfo);
        }

        protected virtual RemoteServiceErrorInfo GetErrorInfo(UserExceptionInformerContext context)
        {
            if (context.Exception is AbpRemoteCallException remoteCallException)
            {
                return remoteCallException.Error;
            }

            return ExceptionToErrorInfoConverter.Convert(context.Exception, false);
        }

        protected virtual async Task ShowErrorInfoAsync(RemoteServiceErrorInfo errorInfo)
        {
            if (errorInfo.Details.IsNullOrEmpty())
            {
                await MessageService.ErrorAsync(errorInfo.Message);
            }
            else
            {
                await MessageService.ErrorAsync(errorInfo.Details, errorInfo.Message);
            }
        }
    }
}
