using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Validation
{
    public class DynamicRangeAttribute : RangeAttribute
    {
        private static readonly FieldInfo MaximumField;
        private static readonly FieldInfo MinimumField;

        static DynamicRangeAttribute()
        {
            MaximumField = typeof(RangeAttribute).GetField(
                "<Maximum>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
            Debug.Assert(MaximumField != null, nameof(MaximumField) + " != null");

            MinimumField = typeof(RangeAttribute).GetField(
                "<Minimum>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
            Debug.Assert(MinimumField != null, nameof(MinimumField) + " != null");
        }

        /// <param name="sourceType">A type to get the values of the properties</param>
        /// <param name="operandType">The type of the range parameters. Must implement IComparable. <see cref="RangeAttribute.OperandType"/></param>
        /// <param name="minimumPropertyName">The name of the public static property for the <see cref="RangeAttribute.Minimum"/></param>
        /// <param name="maximumPropertyName">The name of the public static property for the <see cref="RangeAttribute.Maximum"/></param>
        public DynamicRangeAttribute(
            [NotNull] Type sourceType,
            [NotNull] Type operandType,
            [CanBeNull] string minimumPropertyName,
            [CanBeNull] string maximumPropertyName
        )
            : base(operandType, string.Empty, string.Empty)
        {
            Check.NotNull(sourceType, nameof(sourceType));

            if (minimumPropertyName != null)
            {
                var minimumProperty = sourceType.GetProperty(
                    minimumPropertyName,
                    BindingFlags.Static | BindingFlags.Public
                );
                Debug.Assert(minimumProperty != null, nameof(minimumProperty) + " != null");
                MinimumField.SetValue(this, minimumProperty.GetValue(null));
            }

            if (maximumPropertyName != null)
            {
                var maximumProperty = sourceType.GetProperty(
                    maximumPropertyName,
                    BindingFlags.Static | BindingFlags.Public
                );
                Debug.Assert(maximumProperty != null, nameof(maximumProperty) + " != null");
                MaximumField.SetValue(this, maximumProperty.GetValue(null));
            }
        }
    }
}
