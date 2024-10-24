using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Volo.Abp.Data;

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
                Text = AbpEnumLocalizer.GetString(PropertyInfo.Type, enumValue, new []{ StringLocalizerFactory.CreateDefaultOrNull() })
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
            SelectedValue = (int)PropertyInfo.Type.GetEnumValues().GetValue(0)!;
        }
    }
}

public class SelectItem<TValue>
{
    public string Text { get; set; } = default!;
    public TValue Value { get; set; } = default!;
}
