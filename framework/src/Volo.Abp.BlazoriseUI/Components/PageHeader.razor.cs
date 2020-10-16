using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.WebAssembly;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class PageHeader : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public bool BreadcrumbShowHome { get; set; } = true;

        [Parameter]
        public bool BreadcrumbShowCurrent { get; set; } = true;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected List<BreadcrumbItem> BreadcrumbItems { get; set; }

        public PageHeader()
        {
            BreadcrumbItems = new List<BreadcrumbItem>();
        }

        public void AddBreadcrumbItem(string text, string url = null, string icon = null)
        {
            BreadcrumbItems.Add(new BreadcrumbItem(text, url, icon));
            StateHasChanged();
        }
    }
}
