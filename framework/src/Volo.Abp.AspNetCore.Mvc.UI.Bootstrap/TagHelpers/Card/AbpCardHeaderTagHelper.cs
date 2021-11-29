namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card
{
    public class AbpCardHeaderTagHelper : AbpTagHelper<AbpCardHeaderTagHelper, AbpCardHeaderTagHelperService>
    {
        public AbpCardHeaderTagHelper(AbpCardHeaderTagHelperService tagHelperService) 
            : base(tagHelperService)
        {
        }
    }
}