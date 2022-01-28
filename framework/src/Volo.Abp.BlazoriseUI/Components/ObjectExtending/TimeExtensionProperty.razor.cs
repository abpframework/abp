using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazorise;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class TimeExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected TimeSpan? Value {
        get {
            return PropertyInfo.GetInputValueOrDefault<TimeSpan?>(Entity.GetProperty(PropertyInfo.Name));
        }
        set {
            Entity.SetProperty(PropertyInfo.Name, value, false);
        }
    }
}
