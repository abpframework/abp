namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Carousel
{
    public class AbpCarouselItemTagHelper : AbpTagHelper<AbpCarouselItemTagHelper, AbpCarouselItemTagHelperService>
    {
        public bool? Active { get; set; }

        public string Src { get; set; }

        public string Alt { get; set; }

        public string CaptionTitle { get; set; }

        public string Caption { get; set; }

        public AbpCarouselItemTagHelper(AbpCarouselItemTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
