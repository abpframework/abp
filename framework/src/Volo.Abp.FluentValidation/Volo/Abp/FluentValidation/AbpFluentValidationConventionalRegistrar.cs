using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.FluentValidation
{
    public class AbpFluentValidationConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidator<>)) ||
                   base.IsConventionalRegistrationDisabled(type);
        }

        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }

        protected override List<Type> GetExposedServiceTypes(Type type)
        {
            return new List<Type>()
            {
                typeof(IValidator<>).MakeGenericType(GetFirstGenericArgumentOrNull(type, 1))
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
