using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.FluentValidation
{
    public class AbpFluentValidationConventionalRegistrar : DefaultConventionalRegistrar
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (!typeof(IValidator).IsAssignableFrom(type))
            {
                return;
            }

            var validatingType = GetFirstGenericArgumentOrNull(type, 1);
            if (validatingType == null)
            {
                return;
            }

            services.AddTransient(
                typeof(IValidator<>).MakeGenericType(validatingType),
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
