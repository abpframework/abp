using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Validation
{
    public class DynamicMaxLengthAttribute : MaxLengthAttribute
    {
        private static readonly FieldInfo MaximumLengthField;
        
        static DynamicMaxLengthAttribute()
        {
            MaximumLengthField = typeof(MaxLengthAttribute).GetField(
                "<Length>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
            Debug.Assert(MaximumLengthField != null, nameof(MaximumLengthField) + " != null");
        }

        public DynamicMaxLengthAttribute(
            [NotNull] Type sourceType,
            [CanBeNull] string maximumLengthPropertyName)
        {
            Check.NotNull(sourceType, nameof(sourceType));
            
            if (maximumLengthPropertyName != null)
            {
                var maximumLengthProperty = sourceType.GetProperty(
                    maximumLengthPropertyName,
                    BindingFlags.Static | BindingFlags.Public
                );
                Debug.Assert(maximumLengthProperty != null, nameof(maximumLengthProperty) + " != null");
                MaximumLengthField.SetValue(this, (int) maximumLengthProperty.GetValue(null));
            }
        }
    }
}