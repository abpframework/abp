using System.Collections.Generic;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace AutoMapper;

public static class AbpAutoMapperExtensibleDtoExtensions
{
    public static IMappingExpression<TSource, TDestination> MapExtraProperties<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> mappingExpression,
        MappingPropertyDefinitionChecks? definitionChecks = null,
        string[] ignoredProperties = null,
        bool mapToRegularProperties = false)
        where TDestination : IHasExtraProperties
        where TSource : IHasExtraProperties
    {
        return mappingExpression
            .ForMember(
                x => x.ExtraProperties,
                y => y.MapFrom(
                    (source, destination, extraProps) =>
                    {
                        var result = extraProps.IsNullOrEmpty()
                            ? new Dictionary<string, object>()
                            : new Dictionary<string, object>(extraProps);

                        ExtensibleObjectMapper
                            .MapExtraPropertiesTo<TSource, TDestination>(
                                source.ExtraProperties,
                                result,
                                definitionChecks,
                                ignoredProperties
                            );

                        return result;
                    })
            )
            .AfterMap((source, destination, context) =>
            {
                if (mapToRegularProperties)
                {
                    destination.SetExtraPropertiesToRegularProperties();
                }
            });
    }

    public static IMappingExpression<TSource, TDestination> IgnoreExtraProperties<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> mappingExpression)
        where TDestination : IHasExtraProperties
        where TSource : IHasExtraProperties
    {
        return mappingExpression.Ignore(x => x.ExtraProperties);
    }
}
