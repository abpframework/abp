using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement
{
    public abstract class SettingManagementBase : OwningComponentBase
    {
        [Inject]
        protected IServiceProvider ServiceProvider { get; set; }
        
        protected SettingComponentCreationContext SettingComponentCreationContext { get; set; }

        [Inject]
        protected IOptions<SettingManagementComponentOptions> _options { get; set; }

        protected SettingManagementComponentOptions Options => _options.Value;
        
        protected List<RenderFragment> SettingItemRenders { get; set; } = new List<RenderFragment>();

        protected string SelectedGroup;
        
        protected override async Task OnInitializedAsync()
        {
            SettingComponentCreationContext = new SettingComponentCreationContext(ServiceProvider);

            foreach (var contributor in Options.Contributors)
            {
                await contributor.ConfigureAsync(SettingComponentCreationContext);
            }
            
            SettingItemRenders.Clear();

            SelectedGroup = GetNormalizedString(SettingComponentCreationContext.Groups.First().Id);
        }

        protected string GetNormalizedString(string value)
        {
            return value.Replace('.', '_');
        }
    }
}