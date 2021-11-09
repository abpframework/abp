using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class TimeExtensionProperty<TEntity, TResourceType> : ComponentBase
    where TEntity : IHasExtraProperties
{
    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; }

    [Parameter]
    public TEntity Entity { get; set; }

    [Parameter]
    public ObjectExtensionPropertyInfo PropertyInfo { get; set; }

    protected TimeSpan? Value
    {
        get
        {
            return PropertyInfo.GetInputValueOrDefault<TimeSpan?>(Entity.GetProperty(PropertyInfo.Name));
        }
        set
        {
            Entity.SetProperty(PropertyInfo.Name, value, false);
        }
    }
}
