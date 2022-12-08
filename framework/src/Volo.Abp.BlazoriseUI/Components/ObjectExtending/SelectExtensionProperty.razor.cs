using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Blazorise;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class SelectExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected List<SelectItem<int>> SelectItems = new();

    public int SelectedValue {
        get { return Entity.GetProperty<int>(PropertyInfo.Name); }
        set { Entity.SetProperty(PropertyInfo.Name, value, false); }
    }

    protected virtual List<SelectItem<int>> GetSelectItemsFromEnum()
    {
        var selectItems = new List<SelectItem<int>>();

        foreach (var enumValue in PropertyInfo.Type.GetEnumValues())
        {
            selectItems.Add(new SelectItem<int>
            {
                Value = (int)enumValue,
                Text = EnumHelper.GetLocalizedMemberName(PropertyInfo.Type, enumValue, StringLocalizerFactory)
            });
        }

        return selectItems;
    }

    protected override void OnParametersSet()
    {
        SelectItems = GetSelectItemsFromEnum();
        StateHasChanged();

        if (!Entity.HasProperty(PropertyInfo.Name))
        {
            SelectedValue = (int)PropertyInfo.Type.GetEnumValues().GetValue(0);
        }
    }
}

public class SelectItem<TValue>
{
    public string Text { get; set; }
    public TValue Value { get; set; }
}
