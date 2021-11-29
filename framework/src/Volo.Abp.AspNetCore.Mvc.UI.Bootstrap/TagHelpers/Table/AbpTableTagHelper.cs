namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Table
{
    public class AbpTableTagHelper : AbpTagHelper<AbpTableTagHelper, AbpTableTagHelperService>
    {
        public bool? Responsive { get; set; }
        public bool? ResponsiveSm { get; set; }
        public bool? ResponsiveMd { get; set; }
        public bool? ResponsiveLg { get; set; }
        public bool? ResponsiveXl { get; set; }

        public bool? DarkTheme { get; set; }

        public bool? StripedRows { get; set; }

        public bool? HoverableRows { get; set; }

        public bool? Small { get; set; }

        public AbpTableBorderStyle BorderStyle { get; set; } = AbpTableBorderStyle.Default;

        public AbpTableTagHelper(AbpTableTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
