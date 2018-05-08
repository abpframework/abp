namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.ProgressBar
{
    public class AbpProgressBarTagHelper : AbpTagHelper<AbpProgressBarTagHelper, AbpProgressBarTagHelperService>
    {
        public double Value { get; set; }

        public double MinValue { get; set; } = 0;

        public double MaxValue { get; set; } = 100;

        public AbpProgressBarType Type { get; set; } = AbpProgressBarType.Default;

        public bool? Strip { get; set; }

        public bool? Animation { get; set; }

        public AbpProgressBarTagHelper(AbpProgressBarTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
