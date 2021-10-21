using System;
using System.Collections.Generic;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.FluentValidation
{
    public class AbpFluentValidationConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !typeof(IValidator).IsAssignableFrom(type) || base.IsConventionalRegistrationDisabled(type);
        }

        protected override ServiceLifetime? GetLifeTimeOrNull(Type type, DependencyAttribute dependencyAttribute)
        {
            return base.GetLifeTimeOrNull(type, dependencyAttribute) ?? GetValidatorServiceLifetime(type);
        }

        protected virtual ServiceLifetime GetValidatorServiceLifetime(Type type)
        {
            return ServiceLifetime.Transient;
        }

        protected override List<Type> GetExposedServiceTypes(Type type)
        {
            var validatingType = GetFirstGenericArgumentOrNull(type, 1);
            if (validatingType == null)
            {
                return new List<Type>();
            }

            return new List<Type>()
            {
                typeof(IValidator<>).MakeGenericType(validatingType)
            };
        }

        private static Type GetFirstGenericArgumentOrNull(Type type, int depth)
        {
            const int maxFindDepth = 8;

            if (depth >= maxFindDepth)
            {
                return null;
            }

            if (type.IsGenericType && type.GetGenericArguments().Length >= 1)
            {
                return type.GetGenericArguments()[0];
            }

            return GetFirstGenericArgumentOrNull(type.BaseType, depth + 1);
        }
    }
}
