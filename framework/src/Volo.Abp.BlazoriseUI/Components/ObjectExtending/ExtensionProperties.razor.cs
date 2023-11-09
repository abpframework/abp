using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class ExtensionProperties<TEntityType, TResourceType> : ComponentBase
    where TEntityType : IHasExtraProperties
{
    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; } = default!;

    [Parameter]
    public AbpBlazorMessageLocalizerHelper<TResourceType> LH { get; set; } = default!;

    [Parameter]
    public TEntityType Entity { get; set; } = default!;
    
    [Parameter]
    public ExtensionPropertyModalType? ModalType { get; set; }
}
