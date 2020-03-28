using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public static class HasExtraPropertiesObjectExtendingExtensions
    {
        public static void MapExtraPropertiesTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : IHasExtraProperties
            where TDestination : IHasExtraProperties
        {
            var extensionPropertyInfos = ObjectExtensionManager.Instance.For<TSource>().GetProperties();

            foreach (var extensionPropertyInfo in extensionPropertyInfos)
            {
                if (source.ExtraProperties.ContainsKey(extensionPropertyInfo.Name))
                {
                    destination.ExtraProperties[extensionPropertyInfo.Name] = source.ExtraProperties[extensionPropertyInfo.Name];
                }
            }
        }
    }
}
