namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Breadcrumb
{
    public class AbpBreadcrumbItemTagHelper : AbpTagHelper<AbpBreadcrumbItemTagHelper, AbpBreadcrumbItemTagHelperService>
    {
        public string Href { get; set; }

        public string Title { get; set; }

        public bool Active { get; set; }

        public AbpBreadcrumbItemTagHelper(AbpBreadcrumbItemTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
