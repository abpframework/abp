using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using ProductManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ProductManagement.Pages.ProductManagement
{
    public abstract class ProductManagementPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<ProductManagementResource> L { get; set; }

        public const string DefaultTitle = "ProductManagement";

        public string GetTitle(string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return DefaultTitle;
            }

            return title;
        }
    }
}
