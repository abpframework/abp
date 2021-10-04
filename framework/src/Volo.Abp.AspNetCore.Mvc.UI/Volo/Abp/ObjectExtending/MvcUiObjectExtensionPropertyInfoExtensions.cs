using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Reflection;

namespace Volo.Abp.ObjectExtending
{
    public static class MvcUiObjectExtensionPropertyInfoExtensions
    {
        private static readonly HashSet<Type> NumberTypes = new HashSet<Type> {
            typeof(int),
            typeof(long),
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(int?),
            typeof(long?),
            typeof(byte?),
            typeof(sbyte?),
            typeof(short?),
            typeof(ushort?),
            typeof(uint?),
            typeof(long?),
            typeof(ulong?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?)
        };

        public static string GetInputFormatOrNull(this IBasicObjectExtensionPropertyInfo property)
        {
            var formatString = property.GetDataFormatStringOrNull();

            if (!formatString.IsNullOrWhiteSpace())
            {
                return formatString;
            }

            if (property.IsDate())
            {
                return "{0:yyyy-MM-dd}";
            }

            if (property.IsDateTime())
            {
                return "{0:yyyy-MM-ddTHH:mm}";
            }

            return null;
        }

        public static string GetInputValueOrNull(this IBasicObjectExtensionPropertyInfo property, object value)
        {
            if (value == null)
            {
                return null;
            }

            if (TypeHelper.IsFloatingType(property.Type))
            {
                return value.ToString()?.Replace(',', '.');
            }

            /* Let the ASP.NET Core handle it! */
            return null;
        }

        public static string GetInputType(this ObjectExtensionPropertyInfo propertyInfo)
        {
            foreach (var attribute in propertyInfo.Attributes)
            {
                var inputTypeByAttribute = GetInputTypeFromAttributeOrNull(attribute);
                if (inputTypeByAttribute != null)
                {
                    return inputTypeByAttribute;
                }
            }

            return GetInputTypeFromTypeOrNull(propertyInfo.Type)
                   ?? "text"; //default
        }

        private static string GetInputTypeFromAttributeOrNull(Attribute attribute)
        {
            if (attribute is EmailAddressAttribute)
            {
                return "email";
            }

            if (attribute is UrlAttribute)
            {
                return "url";
            }

            if (attribute is HiddenInputAttribute)
            {
                return "hidden";
            }

            if (attribute is PhoneAttribute)
            {
                return "tel";
            }

            if (attribute is DataTypeAttribute dataTypeAttribute)
            {
                switch (dataTypeAttribute.DataType)
                {
                    case DataType.Password:
                        return "password";
                    case DataType.Date:
                        return "date";
                    case DataType.Time:
                        return "time";
                    case DataType.EmailAddress:
                        return "email";
                    case DataType.Url:
                        return "url";
                    case DataType.PhoneNumber:
                        return "tel";
                    case DataType.DateTime:
                        return "datetime-local";
                }
            }

            return null;
        }

        private static string GetInputTypeFromTypeOrNull(Type type)
        {
            if (type == typeof(bool))
            {
                return "checkbox";
            }

            if (type == typeof(DateTime))
            {
                return "datetime-local";
            }

            if (NumberTypes.Contains(type))
            {
                return "number";
            }

            return null;
        }
    }
}
