using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    [HtmlTargetElement("abp-script", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpScriptTagHelper : AbpTagHelper<AbpScriptTagHelper, AbpScriptTagHelperService>, IBundleTagHelper
    {
        /// <summary>
        /// A file path.
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// A bundle contributor type.
        /// </summary>
        public Type Type { get; set; }

        public AbpScriptTagHelper(AbpScriptTagHelperService service)
            : base(service)
        {

        }

        public string GetNameOrNull()
        {
            if (Type != null)
            {
                return Type.FullName;
            }

            if (Src != null)
            {
                return Src
                    .RemovePreFix("/")
                    .RemovePostFix(StringComparison.OrdinalIgnoreCase, ".js")
                    .Replace("/", ".");
            }

            throw new AbpException("abp-script tag helper requires to set either src or type!");
        }

        public BundleTagHelperItem CreateBundleTagHelperItem()
        {
            if (Type != null)
            {
                return new BundleTagHelperItem(Type);
            }

            if (Src != null)
            {
                return new BundleTagHelperItem(Src);
            }

            throw new AbpException("abp-script tag helper requires to set either src or type!");
        }
    }
}