namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpStyleTagHelperService : AbpBundleItemTagHelperService<AbpStyleTagHelper>
    {
        public AbpStyleTagHelperService(AbpTagHelperStyleService resourceService)
            : base(resourceService)
        {
        }
    }
}