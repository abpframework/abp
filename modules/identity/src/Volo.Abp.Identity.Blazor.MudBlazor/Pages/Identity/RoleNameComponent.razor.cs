using Microsoft.AspNetCore.Components;

namespace Volo.Abp.Identity.Blazor.MudBlazor.Pages.Identity;

public partial class RoleNameComponent : ComponentBase
{
    [Parameter] public object Data { get; set; } = default!;
}
