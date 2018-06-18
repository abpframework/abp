using Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers.Internal;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpScriptTagHelperService : AbpTagHelperResourceItemService<AbpScriptTagHelper>
    {
        public AbpScriptTagHelperService(AbpTagHelperScriptService resourceService)
            : base(resourceService)
        {
        }
    }
}