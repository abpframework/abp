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
            if (typeof(IValidator).IsAssignableFrom(type))
            {
                var dtoType = GetFirstGenericArgumentOrNull(type, 1);
                if (dtoType != null)
                {
                    var serverType = typeof(IValidator<>).MakeGenericType(dtoType);
                    var serviceDescriptor = ServiceDescriptor.Describe(serverType, type, ServiceLifetime.Transient);

                    services.Add(serviceDescriptor);
                }
            }
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
