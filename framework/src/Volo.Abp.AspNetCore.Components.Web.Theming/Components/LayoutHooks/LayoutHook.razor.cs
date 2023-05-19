using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.Ui.LayoutHooks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Components.LayoutHooks;

public partial class LayoutHook : ComponentBase
{
    [Parameter]
    public string Name { get; set; }
    
    [Parameter]
    public string Layout { get; set; }

    [Inject]
    protected IOptions<AbpLayoutHookOptions> LayoutHookOptions { get; set; }

    protected LayoutHookViewModel LayoutHookViewModel { get; private set; }

    protected override Task OnInitializedAsync()
    {
        if (LayoutHookOptions.Value.Hooks.TryGetValue(Name, out var layoutHooks))
        {
            layoutHooks = layoutHooks
                .Where(IsComponentBase)
                .WhereIf(string.IsNullOrWhiteSpace(Layout), x => x.Layout == Layout)
                .ToList();
        }

        layoutHooks ??= new List<LayoutHookInfo>();
        
        LayoutHookViewModel = new LayoutHookViewModel(layoutHooks.ToArray(), Layout);
        
        return Task.CompletedTask;
    }

    protected virtual bool IsComponentBase(LayoutHookInfo layoutHook)
    {
        return typeof(ComponentBase).IsAssignableFrom(layoutHook.ComponentType);
    }
}