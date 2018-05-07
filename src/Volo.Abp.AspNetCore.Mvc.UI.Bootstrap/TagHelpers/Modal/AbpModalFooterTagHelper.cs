namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalFooterTagHelper : AbpTagHelper<AbpModalFooterTagHelper, AbpModalFooterTagHelperService>
    {
        public AbpModalButtons Buttons { get; set; }

        public AbpModalFooterTagHelper(AbpModalFooterTagHelperService tagHelperService)
            : base(tagHelperService)
        {
        }
    }
}