using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Volo.Abp.ObjectExtending
{
    public static class MvcUiObjectExtensionPropertyInfoExtensions
    {
        private static readonly Type[] DateTypes = new[]
        {
            typeof(DateTime), typeof(DateTimeOffset)
        };

        public static string GetInputFormatOrNull(this ObjectExtensionPropertyInfo property)
        {
            if (IsDate(property))
            {
                return "{0:yyyy-MM-dd}";
            }

            return null;
        }

        private static bool IsDate(ObjectExtensionPropertyInfo property)
        {
            if (!DateTypes.Contains(property.Type))
            {
                return false;
            }

            var dataTypeAttribute = property
                .Attributes
                .OfType<DataTypeAttribute>()
                .FirstOrDefault();

            if (dataTypeAttribute == null)
            {
                return false;
            }

            return dataTypeAttribute.DataType == DataType.Date;
        }
    }
}
