using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Reflection;

namespace Volo.Abp.ObjectExtending
{
    public static class ExtensionPropertyHelper
    {
        public static IEnumerable<Attribute> GetDefaultAttributes(Type type)
        {
            if (TypeHelper.IsNonNullablePrimitiveType(type) || type.IsEnum)
            {
                yield return new RequiredAttribute();
            }

            if (type.IsEnum)
            {
                yield return new EnumDataTypeAttribute(type);
            }
        }
    }
}