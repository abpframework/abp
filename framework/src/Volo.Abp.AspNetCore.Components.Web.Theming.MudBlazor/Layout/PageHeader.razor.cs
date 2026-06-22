using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Layout;

public partial class PageHeader : ComponentBase, IDisposable
{
    protected List<RenderFragment> ToolbarItemRenders { get; set; }

    private List<BreadcrumbItem> _breadcrumbItems = new();

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

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        
        // Build MudBlazor breadcrumb items
        BuildBreadcrumbItems();
        
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

    private void BuildBreadcrumbItems()
    {
        _breadcrumbItems.Clear();
        
        if (BreadcrumbShowHome)
        {
            _breadcrumbItems.Add(new BreadcrumbItem(string.Empty, href: "/", icon: Icons.Material.Filled.Home));
        }
        
        var pageItems = PageLayout.BreadcrumbItems.ToList();
        for (var i = 0; i < pageItems.Count; i++)
        {
            var item = pageItems[i];
            var isLast = i == pageItems.Count - 1;
            
                var mudIcon = ConvertToMudIcon(item.Icon);
            
            // If BreadcrumbShowCurrent is false, skip the last item or make it non-clickable
            if (isLast && !BreadcrumbShowCurrent)
            {
                continue;
            }
            
            _breadcrumbItems.Add(new BreadcrumbItem(
                item.Text, 
                href: isLast ? null : item.Href,
                disabled: isLast,
                icon: mudIcon));
        }
    }

    private static string? ConvertToMudIcon(object? icon)
    {
        if (icon == null)
        {
            return null;
        }

        if (icon is string iconString)
        {
            return iconString;
        }

        var iconName = icon.ToString();
        
        return iconName switch
        {
            "Home" => Icons.Material.Filled.Home,
            "Edit" => Icons.Material.Filled.Edit,
            "Delete" => Icons.Material.Filled.Delete,
            "Add" => Icons.Material.Filled.Add,
            "Search" => Icons.Material.Filled.Search,
            "Settings" => Icons.Material.Filled.Settings,
            "User" => Icons.Material.Filled.Person,
            "Users" => Icons.Material.Filled.People,
            "File" => Icons.Material.Filled.InsertDriveFile,
            "Folder" => Icons.Material.Filled.Folder,
            "Dashboard" => Icons.Material.Filled.Dashboard,
            "List" => Icons.Material.Filled.List,
            "Check" => Icons.Material.Filled.Check,
            "Times" => Icons.Material.Filled.Close,
            "Warning" => Icons.Material.Filled.Warning,
            "Info" => Icons.Material.Filled.Info,
            "Question" => Icons.Material.Filled.Help,
            _ => null // Return null for unmapped icons
        };
    }
    
    protected virtual bool ShouldRenderToolbarItems(PageToolbarItem[] items)
    {
        if (items.Length != PageLayout.ToolbarItems.Count)
        {
            return true;
        }

        return items.Where((t, i) => t.ComponentType != PageLayout.ToolbarItems[i].ComponentType).Any();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
    
    public void Dispose()
    {
        PageLayout.Reset();
        ToolbarItemRenders.Clear();
    }
}
