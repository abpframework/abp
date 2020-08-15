using System;

namespace Volo.Abp.GlobalFeatures
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresGlobalFeatureAttribute : Attribute
    {
        public Type FeatureType { get; }

        public RequiresGlobalFeatureAttribute(Type featureType)
        {
            FeatureType = featureType;
        }
    }
}
