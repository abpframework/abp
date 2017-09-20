using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Http;
using Volo.Abp.Ui;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class DefaultExceptionToErrorInfoConverter : IExceptionToErrorInfoConverter, ITransientDependency
    {
        public bool SendAllExceptionsToClients { get; set; } = false;
                
        public RemoteServiceErrorInfo Convert(Exception exception)
        {
            var errorInfo = CreateErrorInfoWithoutCode(exception);

            if (exception is IHasErrorCode)
            {
                errorInfo.Code = (exception as IHasErrorCode).Code;
            }

            return errorInfo;
        }

        private RemoteServiceErrorInfo CreateErrorInfoWithoutCode(Exception exception)
        {
            if (SendAllExceptionsToClients)
            {
                return CreateDetailedErrorInfoFromException(exception);
            }

            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerException is UserFriendlyException ||
                    aggException.InnerException is AbpValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (exception is UserFriendlyException)
            {
                var userFriendlyException = exception as UserFriendlyException;
                return new RemoteServiceErrorInfo(userFriendlyException.Message, userFriendlyException.Details);
            }

            if (exception is AbpValidationException)
            {
                return new RemoteServiceErrorInfo(L("ValidationError"))
                {
                    ValidationErrors = GetValidationErrorInfos(exception as AbpValidationException),
                    Details = GetValidationErrorNarrative(exception as AbpValidationException)
                };
            }

            if (exception is EntityNotFoundException)
            {
                var entityNotFoundException = exception as EntityNotFoundException;

                if (entityNotFoundException.EntityType != null)
                {
                    return new RemoteServiceErrorInfo(
                        string.Format(
                            L("EntityNotFound"),
                            entityNotFoundException.EntityType.Name,
                            entityNotFoundException.Id
                        )
                    );
                }

                return new RemoteServiceErrorInfo(
                    entityNotFoundException.Message
                );
            }

            if (exception is AbpAuthorizationException)
            {
                var authorizationException = exception as AbpAuthorizationException;
                return new RemoteServiceErrorInfo(authorizationException.Message);
            }

            return new RemoteServiceErrorInfo(L("InternalServerError"));
        }


        private RemoteServiceErrorInfo CreateDetailedErrorInfoFromException(Exception exception)
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

        private void AddExceptionToDetails(Exception exception, StringBuilder detailBuilder)
        {
            //Exception Message
            detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

            //Additional info for UserFriendlyException
            if (exception is UserFriendlyException)
            {
                var userFriendlyException = exception as UserFriendlyException;
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

        private RemoteServiceValidationErrorInfo[] GetValidationErrorInfos(AbpValidationException validationException)
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

        private string GetValidationErrorNarrative(AbpValidationException validationException)
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

        private string L(string name)
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