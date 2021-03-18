using System;
using Volo.Abp.Reflection;

namespace Volo.Abp.GlobalFeatures
{
    public static class GlobalFeatureHelper
    {
        public static bool IsGlobalFeatureEnabled(Type controllerType, out RequiresGlobalFeatureAttribute attribute)
        {
            attribute = ReflectionHelper.GetSingleAttributeOrDefault<RequiresGlobalFeatureAttribute>(controllerType);
            return attribute == null || GlobalFeatureManager.Instance.IsEnabled(attribute.GetFeatureName());
        }
    }
}
