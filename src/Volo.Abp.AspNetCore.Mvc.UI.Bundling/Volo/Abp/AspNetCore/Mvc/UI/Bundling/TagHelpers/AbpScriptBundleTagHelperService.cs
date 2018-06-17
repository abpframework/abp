namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpScriptBundleTagHelperService : AbpBundleTagHelperServiceBase<AbpScriptBundleTagHelper>
    {
        public AbpScriptBundleTagHelperService(AbpTagHelperScriptHelper resourceHelper) 
            : base(resourceHelper)
        {
        }
    }
}