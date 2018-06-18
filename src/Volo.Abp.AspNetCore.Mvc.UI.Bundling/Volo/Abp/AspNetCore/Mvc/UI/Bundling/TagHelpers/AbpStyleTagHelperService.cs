namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpStyleTagHelperService : AbpTagHelperResourceItemService<AbpStyleTagHelper>
    {
        public AbpStyleTagHelperService(AbpTagHelperStyleService resourceService)
            : base(resourceService)
        {
        }
    }
}