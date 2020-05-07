using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ExtensionPropertyAttributeDto
    {
        public string Type { get; set; }
        public string TypeSimple { get; set; }
        public Dictionary<string, object> Configuration { get; set; }

        public static ExtensionPropertyAttributeDto Create(Attribute attribute)
        {
            var attributeType = attribute.GetType();
            var dto = new ExtensionPropertyAttributeDto
            {
                Type = TypeHelper.GetFullNameHandlingNullableAndGenerics(attributeType),
                TypeSimple = TypeHelper.GetSimplifiedName(attributeType),
                Configuration = new Dictionary<string, object>()
            };

            if (attribute is StringLengthAttribute stringLengthAttribute)
            {
                dto.Configuration["MaximumLength"] = stringLengthAttribute.MaximumLength;
                dto.Configuration["MinimumLength"] = stringLengthAttribute.MinimumLength;
            }

            //TODO: Others!

            return dto;
        }
    }
}