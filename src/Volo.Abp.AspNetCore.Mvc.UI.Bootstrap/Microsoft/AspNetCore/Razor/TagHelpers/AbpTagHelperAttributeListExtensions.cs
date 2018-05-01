using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers
{
    public static class AbpTagHelperAttributeListExtensions
    {
        public static void AddClass(this TagHelperAttributeList attributes, string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return;
            }

            var classAttribute = attributes["class"];
            if (classAttribute == null)
            {
                attributes.Add("class", className);
            }
            else
            {
                //TODO: Check if it's already added before!
                attributes.SetAttribute("class", classAttribute.Value + " " + className);
            }
        }

        public static void RemoveClass(this TagHelperAttributeList attributes, string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return;
            }

            var classAttribute = attributes["class"];
            if (classAttribute == null)
            {
                return;
            }

            //Can be optimized?
            attributes.SetAttribute("class", classAttribute.Value.ToString().Split(' ').RemoveAll(c => c == className).JoinAsString(" "));
        }
    }
}