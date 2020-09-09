using System;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresGlobalFeatureAttribute : Attribute
    {
        public Type Type { get; }

        public string Name { get; }

        public RequiresGlobalFeatureAttribute([NotNull] Type type)
        {
            Type = Check.NotNull(type, nameof(type));
        }

        public RequiresGlobalFeatureAttribute([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public virtual string GetFeatureName()
        {
            return Name ?? GlobalFeatureNameAttribute.GetName(Type);
        }
    }
}
