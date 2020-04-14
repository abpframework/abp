using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace AutoMapper
{
    public static class AbpAutoMapperExtensibleDtoExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapExtraProperties<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            MappingPropertyDefinitionChecks? definitionChecks = null,
            string[] ignoredProperties = null)
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
                );
        }
    }
}