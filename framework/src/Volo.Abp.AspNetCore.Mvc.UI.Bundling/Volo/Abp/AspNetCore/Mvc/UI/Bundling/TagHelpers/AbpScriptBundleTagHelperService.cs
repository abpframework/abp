namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpScriptBundleTagHelperService : AbpBundleTagHelperService<AbpScriptBundleTagHelper>
    {
        public AbpScriptBundleTagHelperService(AbpTagHelperScriptService resourceHelper) 
            : base(resourceHelper)
        {
        }
    }
}