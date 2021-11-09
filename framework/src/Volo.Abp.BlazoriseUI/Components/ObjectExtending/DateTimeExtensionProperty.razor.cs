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

public partial class DateTimeExtensionProperty<TEntity, TResourceType> : ComponentBase
    where TEntity : IHasExtraProperties
{
    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; }

    [Parameter]
    public TEntity Entity { get; set; }

    [Parameter]
    public ObjectExtensionPropertyInfo PropertyInfo { get; set; }

    protected DateTime? Value
    {
        get
        {
            return PropertyInfo.GetInputValueOrDefault<DateTime?>(Entity.GetProperty(PropertyInfo.Name));
        }
        set
        {
            Entity.SetProperty(PropertyInfo.Name, value, false);
        }
    }
}
