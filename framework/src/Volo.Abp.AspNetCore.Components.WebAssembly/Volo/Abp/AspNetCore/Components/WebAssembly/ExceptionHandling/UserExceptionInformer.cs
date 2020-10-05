using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.ExceptionHandling
{
    public class UserExceptionInformer : IUserExceptionInformer, ITransientDependency
    {
        private readonly IUiMessageService _messageService;

        public UserExceptionInformer(IUiMessageService messageService)
        {
            _messageService = messageService;
        }

        public virtual async Task InformAsync(UserExceptionInformerContext context)
        {
            if (context.Exception is AbpRemoteCallException remoteCallException)
            {
                await InformAbpRemoteCallExceptionAsync(remoteCallException, context);
            }
            else if (context.Exception is AbpValidationException validationException)
            {
                await InformAbpValidationExceptionAsync(validationException, context);
            }
        }

        protected virtual async Task InformAbpRemoteCallExceptionAsync(AbpRemoteCallException exception, UserExceptionInformerContext context)
        {
            if (exception.Error.Details.IsNullOrEmpty())
            {
                await _messageService.ErrorAsync(exception.Error.Message);
            }
            else
            {
                await _messageService.ErrorAsync(exception.Error.Details, exception.Error.Message);
            }
        }

        protected virtual async Task InformAbpValidationExceptionAsync(AbpValidationException exception, UserExceptionInformerContext context)
        {
            await _messageService.ErrorAsync(exception.Message);
        }
    }
}
