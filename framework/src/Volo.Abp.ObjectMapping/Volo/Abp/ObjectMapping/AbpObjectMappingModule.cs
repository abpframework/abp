using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.ObjectMapping;

public class AbpObjectMappingModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnExposing(onServiceExposingContext =>
        {
            //Register types for IObjectMapper<TSource, TDestination> if implements
            onServiceExposingContext.ExposedTypes.AddRange(
                ReflectionHelper.GetImplementedGenericTypes(
                    onServiceExposingContext.ImplementationType,
                    typeof(IObjectMapper<,>)
                ).ConvertAll(t => new ServiceIdentifier(t))
            );

            //Register types for IQueryProjector<TSource, TDestination> if implements
            foreach (var serviceType in ReflectionHelper.GetImplementedGenericTypes(
                         onServiceExposingContext.ImplementationType,
                         typeof(IQueryProjector<,>)))
            {
                onServiceExposingContext.ExposedTypes.AddIfNotContains(new ServiceIdentifier(serviceType));
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(
            typeof(IObjectMapper<>),
            typeof(DefaultObjectMapper<>)
        );
    }
}
