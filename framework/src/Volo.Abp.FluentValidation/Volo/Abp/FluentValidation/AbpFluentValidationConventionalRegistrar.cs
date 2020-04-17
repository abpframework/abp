using System;
using System.Collections.Generic;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.FluentValidation
{
    public class AbpFluentValidationConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (IsConventionalRegistrationDisabled(type))
            {
                return;
            }

            if (!typeof(IValidator).IsAssignableFrom(type))
            {
                return;
            }

            var validatingType = GetFirstGenericArgumentOrNull(type, 1);
            if (validatingType == null)
            {
                return;
            }

            var serviceType = typeof(IValidator<>).MakeGenericType(validatingType);

            TriggerServiceExposing(services, type, new List<Type>{ serviceType });

            services.AddTransient(
                serviceType,
                type
            );
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
