using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.ObjectExtending
{
    public static class ObjectExtensionPropertyInfoAspNetCoreMvcExtensions
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
        };

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
