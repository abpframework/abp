using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.ExceptionHandling
{
    public class DefaultExceptionToErrorInfoConverter : IExceptionToErrorInfoConverter, ITransientDependency
    {
        public bool SendAllExceptionsToClients { get; set; } = false;

        protected AbpExceptionLocalizationOptions LocalizationOptions { get; }
        protected IStringLocalizerFactory StringLocalizerFactory { get; }
        protected IStringLocalizer<AbpUiResource> L { get; }
        protected IServiceProvider ServiceProvider { get; }

        public DefaultExceptionToErrorInfoConverter(
            IOptions<AbpExceptionLocalizationOptions> localizationOptions,
            IStringLocalizerFactory stringLocalizerFactory,
            IStringLocalizer<AbpUiResource> abpUiStringLocalizer,
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            StringLocalizerFactory = stringLocalizerFactory;
            L = abpUiStringLocalizer;
            LocalizationOptions = localizationOptions.Value;
        }

        public RemoteServiceErrorInfo Convert(Exception exception)
        {
            var errorInfo = CreateErrorInfoWithoutCode(exception);

            if (exception is IHasErrorCode hasErrorCodeException)
            {
                errorInfo.Code = hasErrorCodeException.Code;
            }

            return errorInfo;
        }

        protected virtual RemoteServiceErrorInfo CreateErrorInfoWithoutCode(Exception exception)
        {
            if (SendAllExceptionsToClients)
            {
                return CreateDetailedErrorInfoFromException(exception);
            }

            exception = TryToGetActualException(exception);

            if (exception is EntityNotFoundException)
            {
                return CreateEntityNotFoundError(exception as EntityNotFoundException);
            }

            if (exception is AbpAuthorizationException)
            {
                var authorizationException = exception as AbpAuthorizationException;
                return new RemoteServiceErrorInfo(authorizationException.Message);
            }

            var errorInfo = new RemoteServiceErrorInfo();
            
            if (exception is IUserFriendlyException)
            {
                errorInfo.Message = exception.Message;
                errorInfo.Details = (exception as IHasErrorDetails)?.Details;
            }

            if (exception is IHasValidationErrors)
            {
                if (errorInfo.Message.IsNullOrEmpty())
                {
                    errorInfo.Message = L["ValidationErrorMessage"];
                }

                if (errorInfo.Details.IsNullOrEmpty())
                {
                    errorInfo.Details = GetValidationErrorNarrative(exception as IHasValidationErrors);
                }

                errorInfo.ValidationErrors = GetValidationErrorInfos(exception as IHasValidationErrors);
            }

            TryToLocalizeExceptionMessage(exception, errorInfo);

            if (errorInfo.Message.IsNullOrEmpty())
            {
                errorInfo.Message = L["InternalServerErrorMessage"];
            }

            return errorInfo;
        }

        protected virtual void TryToLocalizeExceptionMessage(Exception exception, RemoteServiceErrorInfo errorInfo)
        {
            if (exception is ILocalizeErrorMessage localizeErrorMessageException)
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    errorInfo.Message = localizeErrorMessageException.LocalizeMessage(new LocalizationContext(scope.ServiceProvider));
                }

                return;
            }

            if (!(exception is IHasErrorCode exceptionWithErrorCode))
            {
                return;
            }

            if (exceptionWithErrorCode.Code.IsNullOrWhiteSpace() ||
                !exceptionWithErrorCode.Code.Contains(":"))
            {
                return;
            }

            var codeNamespace = exceptionWithErrorCode.Code.Split(':')[0];

            var localizationResourceType = LocalizationOptions.ErrorCodeNamespaceMappings.GetOrDefault(codeNamespace);
            if (localizationResourceType == null)
            {
                return;
            }

            var stringLocalizer = StringLocalizerFactory.Create(localizationResourceType);
            var localizedString = stringLocalizer[exceptionWithErrorCode.Code];
            if (localizedString.ResourceNotFound)
            {
                return;
            }

            var localizedValue = localizedString.Value;

            if (exception.Data != null && exception.Data.Count > 0)
            {
                foreach (var key in exception.Data.Keys)
                {
                    localizedValue = localizedValue.Replace("{" + key + "}", exception.Data[key].ToString());
                }
            }

            errorInfo.Message = localizedValue;
        }

        protected virtual RemoteServiceErrorInfo CreateEntityNotFoundError(EntityNotFoundException exception)
        {
            if (exception.EntityType != null)
            {
                return new RemoteServiceErrorInfo(
                    string.Format(
                        L["EntityNotFoundErrorMessage"],
                        exception.EntityType.Name,
                        exception.Id
                    )
                );
            }

            return new RemoteServiceErrorInfo(exception.Message);
        }

        protected virtual Exception TryToGetActualException(Exception exception)
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;

                if (aggException.InnerException is IUserFriendlyException ||
                    aggException.InnerException is AbpValidationException ||
                    aggException.InnerException is EntityNotFoundException ||
                    aggException.InnerException is AbpAuthorizationException ||
                    aggException.InnerException is IBusinessException)
                {
                    return aggException.InnerException;
                }
            }

            return exception;
        }


        protected virtual RemoteServiceErrorInfo CreateDetailedErrorInfoFromException(Exception exception)
        {
            var detailBuilder = new StringBuilder();

            AddExceptionToDetails(exception, detailBuilder);

            var errorInfo = new RemoteServiceErrorInfo(exception.Message, detailBuilder.ToString());

            if (exception is AbpValidationException)
            {
                errorInfo.ValidationErrors = GetValidationErrorInfos(exception as AbpValidationException);
            }

            return errorInfo;
        }

        protected virtual void AddExceptionToDetails(Exception exception, StringBuilder detailBuilder)
        {
            //Exception Message
            detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

            //Additional info for UserFriendlyException
            if (exception is IUserFriendlyException && 
                exception is IHasErrorDetails)
            {
                var details = ((IHasErrorDetails) exception).Details;
                if (!details.IsNullOrEmpty())
                {
                    detailBuilder.AppendLine(details);
                }
            }

            //Additional info for AbpValidationException
            if (exception is AbpValidationException)
            {
                var validationException = exception as AbpValidationException;
                if (validationException.ValidationErrors.Count > 0)
                {
                    detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
                }
            }

            //Exception StackTrace
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                detailBuilder.AppendLine("STACK TRACE: " + exception.StackTrace);
            }

            //Inner exception
            if (exception.InnerException != null)
            {
                AddExceptionToDetails(exception.InnerException, detailBuilder);
            }

            //Inner exceptions for AggregateException
            if (exception is AggregateException)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerExceptions.IsNullOrEmpty())
                {
                    return;
                }

                foreach (var innerException in aggException.InnerExceptions)
                {
                    AddExceptionToDetails(innerException, detailBuilder);
                }
            }
        }

        protected virtual RemoteServiceValidationErrorInfo[] GetValidationErrorInfos(IHasValidationErrors validationException)
        {
            var validationErrorInfos = new List<RemoteServiceValidationErrorInfo>();

            foreach (var validationResult in validationException.ValidationErrors)
            {
                var validationError = new RemoteServiceValidationErrorInfo(validationResult.ErrorMessage);

                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    validationError.Members = validationResult.MemberNames.Select(m => m.ToCamelCase()).ToArray();
                }

                validationErrorInfos.Add(validationError);
            }

            return validationErrorInfos.ToArray();
        }

        protected virtual string GetValidationErrorNarrative(IHasValidationErrors validationException)
        {
            var detailBuilder = new StringBuilder();
            detailBuilder.AppendLine(L["ValidationNarrativeErrorMessageTitle"]);

            foreach (var validationResult in validationException.ValidationErrors)
            {
                detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
                detailBuilder.AppendLine();
            }

            return detailBuilder.ToString();
        }
    }
}