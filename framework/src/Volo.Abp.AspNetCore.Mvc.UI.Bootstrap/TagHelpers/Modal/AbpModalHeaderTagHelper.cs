namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalHeaderTagHelper : AbpTagHelper<AbpModalHeaderTagHelper, AbpModalHeaderTagHelperService>
    {
        public string Title { get; set; }
        
        public AbpModalHeaderTagHelper(AbpModalHeaderTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}