using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Components;

public partial class DynamicLayoutComponent : ComponentBase
{
    [Inject]
    protected IOptions<AbpDynamicLayoutComponentOptions> AbpDynamicLayoutComponentOptions { get; set; } = default!;
}
