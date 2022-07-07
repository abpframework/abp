using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Components;

public partial class AbpDynamicComponents : ComponentBase
{
    [Inject]
    protected IOptions<AbpDynamicComponentOptions> AbpDynamicComponentOptions { get; set; }
}