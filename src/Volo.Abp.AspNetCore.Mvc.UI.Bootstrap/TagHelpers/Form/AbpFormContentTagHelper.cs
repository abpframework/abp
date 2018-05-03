using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [HtmlTargetElement("abp-form-content", TagStructure = TagStructure.WithoutEndTag)]
    public class AbpFormContentTagHelper : AbpTagHelper<AbpFormContentTagHelper, AbpFormContentTagHelperService>, ITransientDependency
    {
        public AbpFormContentTagHelper(AbpFormContentTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
