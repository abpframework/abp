namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpStyleBundleTagHelperService : AbpBundleTagHelperService<AbpStyleBundleTagHelper>
    {
        public AbpStyleBundleTagHelperService(AbpTagHelperStyleService resourceHelper) 
            : base(resourceHelper)
        {
        }
    }
}