namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Carousel
{
    public class AbpCarouselTagHelper : AbpTagHelper<AbpCarouselTagHelper, AbpCarouselTagHelperService>
    {
        public string Id { get; set; }

        public bool? Crossfade { get; set; }

        public bool? Controls { get; set; }

        public bool? Indicators { get; set; }

        public AbpCarouselTagHelper(AbpCarouselTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
