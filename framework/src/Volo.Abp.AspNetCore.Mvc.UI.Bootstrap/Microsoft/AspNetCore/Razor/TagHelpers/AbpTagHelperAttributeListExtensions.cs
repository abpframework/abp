using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers //TODO: Move to AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers namespace
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
                var existingClasses = classAttribute.Value.ToString().Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                existingClasses.AddIfNotContains(className);
                attributes.SetAttribute("class", string.Join(" ", existingClasses));
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

            var classList = classAttribute.Value.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            classList.RemoveAll(c => c == className);

            attributes.SetAttribute("class", classList.JoinAsString(" "));
        }

        public static void AddIfNotContains(this TagHelperAttributeList attributes, string name, object value)
        {
            if (!attributes.ContainsName(name))
            {
                attributes.Add(name, value);
            }
        }
    }
}
