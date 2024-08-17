using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
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

    [Inject]
    public IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter]
    public ExtensionPropertyModalType? ModalType { get; set; }

    public ImmutableList<ObjectExtensionPropertyInfo> Properties { get; set; } = ImmutableList<ObjectExtensionPropertyInfo>.Empty;

    protected async override Task OnInitializedAsync()
    {
        Properties = await ObjectExtensionManager.Instance.GetPropertiesAndCheckPolicyAsync<TEntityType>(ServiceProvider);
    }
}
