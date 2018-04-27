using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.UI;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class DefaultExceptionToErrorInfoConverter : IExceptionToErrorInfoConverter, ITransientDependency
    {
        public bool SendAllExceptionsToClients { get; set; } = false;

        private readonly ExceptionLocalizationOptions _localizationOptions;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public DefaultExceptionToErrorInfoConverter(
            IOptions<ExceptionLocalizationOptions> localizationOptions,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
            _localizationOptions = localizationOptions.Value;
        }

        public RemoteServiceErrorInfo Convert(Exception exception)
        {
            var errorInfo = CreateErrorInfoWithoutCode(exception);

            if (exception is IHasErrorCode)
            {
                errorInfo.Code = (exception as IHasErrorCode).Code;
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
                var userFriendlyException = exception as IUserFriendlyException;
                errorInfo.Message = exception.Message;
                errorInfo.Details = userFriendlyException.Details;
            }

            if (exception is IHasValidationErrors)
            {
                if (errorInfo.Message.IsNullOrEmpty())
                {
                    errorInfo.Message = L("ValidationError");
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
                errorInfo.Message = L("InternalServerError");
            }

            return errorInfo;
        }

        protected virtual void TryToLocalizeExceptionMessage(Exception exception, RemoteServiceErrorInfo errorInfo)
        {
            //TODO: For test purpose
            if (!(exception is IHasErrorCode exceptionWithErrorCode))
            {
                return;
            }

            if (!exceptionWithErrorCode.Code.Contains(":"))
            {
                return;
            }

            var codeNamespace = exceptionWithErrorCode.Code.Split(':')[0];

            var localizationResourceType = _localizationOptions.CodeNamespaceMappings.GetOrDefault(codeNamespace);
            if (localizationResourceType == null)
            {
                return;
            }

            var stringLocalizer = _stringLocalizerFactory.Create(localizationResourceType);
            var localizedString = stringLocalizer[exceptionWithErrorCode.Code];
            if (localizedString.ResourceNotFound)
            {
                return;
            }

            errorInfo.Message = localizedString.Value;
        }

        protected virtual RemoteServiceErrorInfo CreateEntityNotFoundError(EntityNotFoundException exception)
        {
            if (exception.EntityType != null)
            {
                return new RemoteServiceErrorInfo(
                    string.Format(
                        L("EntityNotFound"),
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
                    aggException.InnerException is BusinessException)
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
            if (exception is IUserFriendlyException)
            {
                var userFriendlyException = exception as IUserFriendlyException;
                if (!string.IsNullOrEmpty(userFriendlyException.Details))
                {
                    detailBuilder.AppendLine(userFriendlyException.Details);
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
            detailBuilder.AppendLine(L("ValidationNarrativeTitle"));

            foreach (var validationResult in validationException.ValidationErrors)
            {
                detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
                detailBuilder.AppendLine();
            }

            return detailBuilder.ToString();
        }

        protected virtual string L(string name)
        {
            //TODO: Localization?
            //try
            //{
            //    return _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, name);
            //}
            //catch (Exception)
            //{
            //    return name;
            //}

            return name;
        }
    }
}