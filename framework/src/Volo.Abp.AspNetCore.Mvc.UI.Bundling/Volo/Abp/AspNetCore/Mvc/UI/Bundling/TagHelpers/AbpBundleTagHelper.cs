using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public abstract class AbpBundleTagHelper<TTagHelper, TService> : AbpTagHelper<TTagHelper, TService>, IBundleTagHelper
        where TTagHelper : AbpTagHelper<TTagHelper, TService>
        where TService : class, IAbpTagHelperService<TTagHelper>
    {
        public string Name { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected AbpBundleTagHelper(TService service)
            : base(service)
        {

        }

        public virtual string GetNameOrNull()
        {
            return Name;
        }
    }
}
