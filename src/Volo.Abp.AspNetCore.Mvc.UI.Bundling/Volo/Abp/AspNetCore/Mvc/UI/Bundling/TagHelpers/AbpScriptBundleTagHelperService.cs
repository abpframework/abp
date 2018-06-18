using Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers.Internal;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpScriptBundleTagHelperService : AbpBundleTagHelperServiceBase<AbpScriptBundleTagHelper>
    {
        public AbpScriptBundleTagHelperService(AbpTagHelperScriptService resourceHelper) 
            : base(resourceHelper)
        {
        }
    }
}