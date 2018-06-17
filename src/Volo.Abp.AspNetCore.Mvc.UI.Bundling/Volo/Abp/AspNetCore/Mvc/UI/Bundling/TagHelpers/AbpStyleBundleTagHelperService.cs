namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpStyleBundleTagHelperService : AbpBundleTagHelperServiceBase<AbpStyleBundleTagHelper>
    {
        public AbpStyleBundleTagHelperService(AbpTagHelperStyleHelper resourceHelper) 
            : base(resourceHelper)
        {
        }
    }
}