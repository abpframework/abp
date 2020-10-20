using System.Collections.Generic;
using Blazorise;
using Microsoft.AspNetCore.Components;

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

        [Parameter]
        public List<BreadcrumbItem> BreadcrumbItems { get; set; }

        public PageHeader()
        {
            BreadcrumbItems = new List<BreadcrumbItem>();
        }
    }
}
