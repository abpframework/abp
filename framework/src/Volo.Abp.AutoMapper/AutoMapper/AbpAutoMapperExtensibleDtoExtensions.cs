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
            return mappingExpression
                .ForMember(
                    x => x.ExtraProperties,
                    y => y.MapFrom(
                        (source, dto, extraProps) =>
                        {
                            var result = extraProps.IsNullOrEmpty()
                                ? new Dictionary<string, object>()
                                : new Dictionary<string, object>(extraProps);

                            var objectExtension = ObjectExtensionManager.Instance.GetOrNull<TDestination>();

                            if (objectExtension != null)
                            {
                                foreach (var property in objectExtension.GetProperties())
                                {
                                    if (source.ExtraProperties.ContainsKey(property.Name))
                                    {
                                        result[property.Name] = source.ExtraProperties[property.Name];
                                    }
                                }
                            }

                            return result;
                        })
                );
        }
    }
}
