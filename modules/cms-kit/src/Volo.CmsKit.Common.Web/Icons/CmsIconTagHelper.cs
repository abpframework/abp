using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.CmsKit.Web.Icons
{
    [HtmlTargetElement(TagStructure = TagStructure.WithoutEndTag)]
    public class CmsIconTagHelper : AbpTagHelper<CmsIconTagHelper, CmsIconTagHelperService>
    {
        public string Name { get; set; }

        public CmsIconTagHelper(CmsIconTagHelperService service)
            : base(service)
        {
        }
    }
}
