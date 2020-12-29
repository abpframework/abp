using System;
using System.Linq;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperConventionalRegistrar : ConventionalRegistrarBase
    {
        protected readonly Type[] OpenTypes = {
            typeof(IValueResolver<,,>),
            typeof(IMemberValueResolver<,,,>),
            typeof(ITypeConverter<,>),
            typeof(IValueConverter<,>),
            typeof(IMappingAction<,>)
        };

        public override void AddType(IServiceCollection services, Type type)
        {
            if (IsConventionalRegistrationDisabled(type))
            {
                return;
            }

            if (type.IsClass && !type.IsAbstract && OpenTypes.Any(type.ImplementsGenericInterface))
            {
                services.TryAddTransient(type);
            }
        }
    }
}
