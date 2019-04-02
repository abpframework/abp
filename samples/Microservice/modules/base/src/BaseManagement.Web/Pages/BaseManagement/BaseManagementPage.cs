using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using BaseManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BaseManagement.Pages.BaseManagement
{
    public abstract class BaseManagementPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<BaseManagementResource> L { get; set; }

        public const string DefaultTitle = "BaseManagement";

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
