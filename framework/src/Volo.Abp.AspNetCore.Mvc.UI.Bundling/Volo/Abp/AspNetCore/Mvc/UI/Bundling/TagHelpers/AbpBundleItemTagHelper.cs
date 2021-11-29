using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public abstract class AbpBundleItemTagHelper<TTagHelper, TTagHelperService> : AbpTagHelper<TTagHelper, TTagHelperService>, IBundleItemTagHelper
        where TTagHelper : AbpTagHelper<TTagHelper, TTagHelperService>, IBundleItemTagHelper
        where TTagHelperService: AbpBundleItemTagHelperService<TTagHelper, TTagHelperService>
    {
        /// <summary>
        /// A file path.
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// A bundle contributor type.
        /// </summary>
        public Type Type { get; set; }

        protected AbpBundleItemTagHelper(TTagHelperService service)
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
                    .RemovePostFix(StringComparison.OrdinalIgnoreCase, "." + GetFileExtension())
                    .Replace("/", ".");
            }

            throw new AbpException("abp-script tag helper requires to set either src or type!");
        }

        public BundleTagHelperItem CreateBundleTagHelperItem()
        {
            if (Type != null)
            {
                return new BundleTagHelperContributorTypeItem(Type);
            }

            if (Src != null)
            {
                return new BundleTagHelperFileItem(Src);
            }

            throw new AbpException("abp-script tag helper requires to set either src or type!");
        }

        protected abstract string GetFileExtension();
    }
}
