using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;
using System.Linq;
using System.Collections.ObjectModel;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Layout;

public partial class PageHeader : ComponentBase
{
    protected List<RenderFragment> ToolbarItemRenders { get; set; }

    public IPageToolbarManager PageToolbarManager { get; set; } = default!;

    [Inject]
    public PageLayout PageLayout { get; private set; } = default!;

    [Parameter] // TODO: Consider removing this property in future and use only PageLayout.
    public string? Title { get => PageLayout.Title; set => PageLayout.Title = value; }

    [Parameter]
    public bool BreadcrumbShowHome { get; set; } = true;

    [Parameter]
    public bool BreadcrumbShowCurrent { get; set; } = true;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter] // TODO: Consider removing this property in future and use only PageLayout.
    public List<BreadcrumbItem> BreadcrumbItems {
        get => PageLayout.BreadcrumbItems.ToList();
        set 
        {
            PageLayout.BreadcrumbItems.Clear();
            foreach (var item in value)
            {
                PageLayout.BreadcrumbItems.Add(item);
            }
        }
    }

    [Parameter]
    public PageToolbar? Toolbar { get; set; }

    public PageHeader()
    {
        ToolbarItemRenders = new List<RenderFragment>();
    }

    protected async override Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (Toolbar != null)
        {
            var toolbarItems = await PageToolbarManager.GetItemsAsync(Toolbar);

            if (!ShouldRenderToolbarItems(toolbarItems))
            {
                return;
            }
            
            ToolbarItemRenders.Clear();

            if (!Options.Value.RenderToolbar)
            {
                PageLayout.ToolbarItems.Clear();
                foreach (var item in toolbarItems)
                {
                    PageLayout.ToolbarItems.Add(item);
                }
                return;
            }

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
    
    protected virtual bool ShouldRenderToolbarItems(PageToolbarItem[] items)
    {
        if (items.Length != PageLayout.ToolbarItems.Count)
        {
            return true;
        }

        return items.Where((t, i) => t.ComponentType != PageLayout.ToolbarItems[i].ComponentType).Any();
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}
