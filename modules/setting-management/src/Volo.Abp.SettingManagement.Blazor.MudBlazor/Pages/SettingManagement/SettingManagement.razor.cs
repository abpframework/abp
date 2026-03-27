using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MudBlazor;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.MudBlazor.Pages.SettingManagement;

public partial class SettingManagement
{
    [Inject]
    protected IServiceProvider ServiceProvider { get; set; } = default!;

    protected SettingComponentCreationContext SettingComponentCreationContext { get; set; } = default!;

    [Inject]
    protected IOptions<SettingManagementComponentOptions> _options { get; set; } = default!;
    
    [Inject]
    protected IStringLocalizer<AbpSettingManagementResource> L { get; set; } = default!;

    protected SettingManagementComponentOptions Options => _options.Value;

    protected List<RenderFragment> SettingItemRenders { get; set; } = new List<RenderFragment>();

    protected string? SelectedGroup;
    protected List<BreadcrumbItem> BreadcrumbItems = new();

    protected int _activeTabIndex = 0;

    protected async override Task OnInitializedAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(LUiNavigation["Menu:Administration"].Value,  null, disabled: true));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:Settings"].Value,  null, disabled: true));

        SettingComponentCreationContext = new SettingComponentCreationContext(ServiceProvider);

        foreach (var contributor in Options.Contributors)
        {
            await contributor.ConfigureAsync(SettingComponentCreationContext);
        }
        SettingComponentCreationContext.Normalize();
        SettingItemRenders.Clear();

        if (SelectedGroup.IsNullOrEmpty() && SettingComponentCreationContext.Groups.Any())
        {
            SelectedGroup = GetNormalizedString(SettingComponentCreationContext.Groups.First().Id);
            _activeTabIndex = 0;
        }
    }

    protected virtual string GetNormalizedString(string value)
    {
        return value.Replace('.', '_');
    }
}
