using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.TextTemplating.Razor
{
    public abstract class RazorTemplatePageBase<TModel> : RazorTemplatePageBase, IRazorTemplatePage<TModel>
    {
        public TModel Model { get; set; }
    }

    public abstract class RazorTemplatePageBase : IRazorTemplatePage
    {
        public Dictionary<string, object> GlobalContext { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IStringLocalizer Localizer { get; set; }

        public HtmlEncoder HtmlEncoder { get; set; }

        public JavaScriptEncoder JavaScriptEncoder { get; set; }

        public UrlEncoder UrlEncoder { get; set; }

        public string Body { get; set; }

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private AttributeInfo _attributeInfo;

        public virtual void WriteLiteral(string literal = null)
        {
            if (!literal.IsNullOrEmpty())
            {
                _stringBuilder.Append(literal);
            }
        }

        public virtual void Write(object value = null)
        {
            if (value is null)
            {
                return;
            }

            _stringBuilder.Append(value.ToString());
        }

        public virtual void BeginWriteAttribute(string name, string prefix, int prefixOffset, string suffix, int suffixOffset, int attributeValuesCount)
        {
            _attributeInfo = new AttributeInfo(name, prefix, prefixOffset, suffix, suffixOffset, attributeValuesCount);

            if (attributeValuesCount != 1)
            {
                WriteLiteral(prefix);
            }
        }

        public virtual void WriteAttributeValue(string prefix, int prefixOffset, object value, int valueOffset, int valueLength, bool isLiteral)
        {
            if (_attributeInfo.AttributeValuesCount == 1)
            {
                if (IsBoolFalseOrNullValue(prefix, value))
                {
                    // Value is either null or the bool 'false' with no prefix; don't render the attribute.
                    _attributeInfo.Suppressed = true;
                    return;
                }

                // We are not omitting the attribute. Write the prefix.
                WriteLiteral(_attributeInfo.Prefix);

                if (IsBoolTrueWithEmptyPrefixValue(prefix, value))
                {
                    // The value is just the bool 'true', write the attribute name instead of the string 'True'.
                    value = _attributeInfo.Name;
                }
            }

            // This block handles two cases.
            // 1. Single value with prefix.
            // 2. Multiple values with or without prefix.
            if (value != null)
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    WriteLiteral(prefix);
                }

                WriteUnprefixedAttributeValue(value, isLiteral);
            }
        }

        public virtual void EndWriteAttribute()
        {
            if (!_attributeInfo.Suppressed)
            {
                WriteLiteral(_attributeInfo.Suffix);
            }
        }

        public virtual Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task<string> GetOutputAsync()
        {
            return Task.FromResult(_stringBuilder.ToString());
        }

        private void WriteUnprefixedAttributeValue(object value, bool isLiteral)
        {
            var stringValue = value as string;

            // The extra branching here is to ensure that we call the Write*To(string) overload where possible.
            if (isLiteral && stringValue != null)
            {
                WriteLiteral(stringValue);
            }
            else if (isLiteral)
            {
                //WriteLiteral(value);
                _stringBuilder.Append(value);
            }
            else if (stringValue != null)
            {
                Write(stringValue);
            }
            else
            {
                Write(value);
            }
        }


        private static bool IsBoolFalseOrNullValue(string prefix, object value)
        {
            return string.IsNullOrEmpty(prefix) &&
                   (value is null ||
                    (value is bool boolValue && !boolValue));
        }

        private static bool IsBoolTrueWithEmptyPrefixValue(string prefix, object value)
        {
            // If the value is just the bool 'true', use the attribute name as the value.
            return string.IsNullOrEmpty(prefix) &&
                   (value is bool boolValue && boolValue);
        }

        private struct AttributeInfo
        {
            public AttributeInfo(
                string name,
                string prefix,
                int prefixOffset,
                string suffix,
                int suffixOffset,
                int attributeValuesCount)
            {
                Name = name;
                Prefix = prefix;
                PrefixOffset = prefixOffset;
                Suffix = suffix;
                SuffixOffset = suffixOffset;
                AttributeValuesCount = attributeValuesCount;

                Suppressed = false;
            }

            public int AttributeValuesCount { get; }

            public string Name { get; }

            public string Prefix { get; }

            public int PrefixOffset { get; }

            public string Suffix { get; }

            public int SuffixOffset { get; }

            public bool Suppressed { get; set; }
        }
    }
}
