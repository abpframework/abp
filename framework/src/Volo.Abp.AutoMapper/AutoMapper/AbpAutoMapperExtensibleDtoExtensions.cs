using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace AutoMapper
{
    public static class AbpAutoMapperExtensibleDtoExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapExtraProperties<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
            where TDestination : IHasExtraProperties
            where TSource : IHasExtraProperties
        {
            var properties = ObjectExtensionManager.GetProperties<TDestination>();
            return mappingExpression
                .ForMember(
                    x => x.ExtraProperties,
                    y => y.MapFrom(
                        (source, dto, extraProps) =>
                        {
                            var result = extraProps.IsNullOrEmpty()
                                ? new Dictionary<string, object>()
                                : new Dictionary<string, object>(extraProps);

                            foreach (var property in properties)
                            {
                                result[property.Name] = source.ExtraProperties[property.Name];
                            }

                            return result;
                        })
                );
        }
    }
}
