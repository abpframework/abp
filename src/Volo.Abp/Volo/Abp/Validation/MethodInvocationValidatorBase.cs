using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;

namespace Volo.Abp.Validation
{
    public abstract class MethodInvocationValidatorBase : ITransientDependency
    {
        private readonly IObjectValidator _objectValidator;

        protected MethodInvocationValidatorBase(IObjectValidator objectValidator)
        {
            _objectValidator = objectValidator;
        }

        /// <summary>
        /// Validates the method invocation.
        /// </summary>
        protected virtual void ValidateInternal(MethodInvocationValidationContext context)
        {
            Check.NotNull(context, nameof(context));

            if (context.Parameters.IsNullOrEmpty())
            {
                return;
            }

            if (!context.Method.IsPublic)
            {
                return;
            }

            if (IsValidationDisabled(context))
            {
                return;
            }

            if (context.Parameters.Length != context.ParameterValues.Length)
            {
                throw new Exception("Method parameter count does not match with argument count!");
            }

            if (context.Errors.Any() && HasSingleNullArgument(context))
            {
                ThrowValidationError(context);
            }

            AddMethodParameterValidationErrors(context);

            if (context.Errors.Any())
            {
                ThrowValidationError(context);
            }

            foreach (var objectToBeNormalized in context.ObjectsToBeNormalized)
            {
                objectToBeNormalized.Normalize();
            }
        }

        protected virtual bool IsValidationDisabled(MethodInvocationValidationContext context)
        {
            if (context.Method.IsDefined(typeof(EnableValidationAttribute), true))
            {
                return false;
            }

            return ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(context.Method) != null;
        }

        protected virtual bool HasSingleNullArgument(MethodInvocationValidationContext context)
        {
            return context.Parameters.Length == 1 && context.ParameterValues[0] == null;
        }

        protected virtual void ThrowValidationError(MethodInvocationValidationContext context)
        {
            throw new AbpValidationException(
                "Method arguments are not valid! See ValidationErrors for details.",
                context.Errors
            );
        }

        protected virtual void AddMethodParameterValidationErrors(MethodInvocationValidationContext context)
        {
            for (var i = 0; i < context.Parameters.Length; i++)
            {
                AddMethodParameterValidationErrors(context, context.Parameters[i], context.ParameterValues[i]);
            }
        }

        protected virtual void AddMethodParameterValidationErrors(IAbpValidationResult context, ParameterInfo parameterInfo, object parameterValue)
        {
            if (parameterValue == null)
            {
                if (!parameterInfo.IsOptional &&
                    !parameterInfo.IsOut &&
                    !TypeHelper.IsPrimitiveExtendedIncludingNullable(parameterInfo.ParameterType, includeEnums: true))
                {
                    context.Errors.Add(new ValidationResult(parameterInfo.Name + " is null!", new[] { parameterInfo.Name }));
                }

                return;
            }

            _objectValidator.AddValidatationErrors(context, parameterValue);
        }
    }
}