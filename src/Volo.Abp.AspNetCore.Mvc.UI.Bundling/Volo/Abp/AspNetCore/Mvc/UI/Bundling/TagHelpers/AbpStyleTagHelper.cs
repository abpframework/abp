using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    [HtmlTargetElement("abp-style", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpStyleTagHelper : AbpTagHelper<AbpStyleTagHelper, AbpStyleTagHelperService>, IBundleTagHelper
    {
        /// <summary>
        /// A file path.
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// A bundle contributor type.
        /// </summary>
        public Type Type { get; set; }

        public AbpStyleTagHelper(AbpStyleTagHelperService service)
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
                    .RemovePostFix(StringComparison.OrdinalIgnoreCase, ".css")
                    .Replace("/", ".");
            }

            throw new AbpException("abp-style tag helper requires to set either src or type!");
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

            throw new AbpException("abp-style tag helper requires to set either src or type!");
        }
    }
}