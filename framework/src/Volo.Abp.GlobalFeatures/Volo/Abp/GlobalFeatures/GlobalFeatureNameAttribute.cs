using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GlobalFeatureNameAttribute : Attribute
    {
        [NotNull]
        public string Name { get; }

        public GlobalFeatureNameAttribute([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public static string GetName<TFeature>()
        {
            return GetName(typeof(TFeature));
        }

        [NotNull]
        public static string GetName([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));

            var attribute = type
                .GetCustomAttributes<GlobalFeatureNameAttribute>()
                .FirstOrDefault();

            if (attribute == null)
            {
                throw new AbpException($"{type.AssemblyQualifiedName} should define the {typeof(GlobalFeatureNameAttribute).FullName} atttribute!");
            }

            return attribute
                .As<GlobalFeatureNameAttribute>()
                .Name;
        }
    }
}
