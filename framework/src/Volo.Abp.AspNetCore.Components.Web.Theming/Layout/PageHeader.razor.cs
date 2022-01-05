using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Layout;

public partial class PageHeader : ComponentBase
{
    protected List<RenderFragment> ToolbarItemRenders { get; set; }

    public IPageToolbarManager PageToolbarManager { get; set; }

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

    [Parameter]
    public PageToolbar Toolbar { get; set; }

    public PageHeader()
    {
        BreadcrumbItems = new List<BreadcrumbItem>();
        ToolbarItemRenders = new List<RenderFragment>();
    }

    protected async override Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (Toolbar != null)
        {
            var toolbarItems = await PageToolbarManager.GetItemsAsync(Toolbar);
            ToolbarItemRenders.Clear();

            foreach (var item in toolbarItems)
            {
                var sequence = 0;
                ToolbarItemRenders.Add(builder =>
                {
                    builder.OpenComponent(sequence, item.ComponentType);
                    if (item.Arguments != null)
                    {
                        foreach (var argument in item.Arguments)
                        {
                            sequence++;
                            builder.AddAttribute(sequence, argument.Key, argument.Value);
                        }
                    }
                    builder.CloseComponent();
                });
            }
        }
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}
