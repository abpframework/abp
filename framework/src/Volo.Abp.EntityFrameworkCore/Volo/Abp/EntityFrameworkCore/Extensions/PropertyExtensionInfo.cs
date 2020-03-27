using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.EntityFrameworkCore.Extensions
{
    public class PropertyExtensionInfo
    {
        public Action<PropertyBuilder> Action { get; set; }

        public Type PropertyType { get; }

        public PropertyExtensionInfo(Type propertyType)
        {
            PropertyType = propertyType;
        }
    }
}