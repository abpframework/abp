namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Image
{
    public class AbpImageTagHelper : AbpTagHelper<AbpImageTagHelper, AbpImageTagHelperService>
    {
        public bool? Responsive { get; set; }

        public bool? Thumbnail { get; set; }

        public bool? Rounded { get; set; }

        public AbpImagePosition Position { get; set; } = AbpImagePosition.Default;

        public string Alt { get; set; }

        public string Src { get; set; }

        public AbpImageTagHelper(AbpImageTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
