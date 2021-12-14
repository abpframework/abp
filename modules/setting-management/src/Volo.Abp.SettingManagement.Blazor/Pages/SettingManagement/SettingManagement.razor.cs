using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement;

public partial class SettingManagement
{
    [Inject]
    protected IServiceProvider ServiceProvider { get; set; }

    protected SettingComponentCreationContext SettingComponentCreationContext { get; set; }

    [Inject]
    protected IOptions<SettingManagementComponentOptions> _options { get; set; }
    [Inject]
    protected IStringLocalizer<AbpSettingManagementResource> L { get; set; }

    protected SettingManagementComponentOptions Options => _options.Value;

    protected List<RenderFragment> SettingItemRenders { get; set; } = new List<RenderFragment>();

    protected string SelectedGroup;
    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>();

    protected async override Task OnInitializedAsync()
    {
        SettingComponentCreationContext = new SettingComponentCreationContext(ServiceProvider);

        foreach (var contributor in Options.Contributors)
        {
            await contributor.ConfigureAsync(SettingComponentCreationContext);
        }

        SettingItemRenders.Clear();

        SelectedGroup = GetNormalizedString(SettingComponentCreationContext.Groups.First().Id);
    }

    protected virtual string GetNormalizedString(string value)
    {
        return value.Replace('.', '_');
    }
}
