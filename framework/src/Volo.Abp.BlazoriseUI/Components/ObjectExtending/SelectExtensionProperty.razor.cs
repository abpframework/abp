using System;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class SelectExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected List<SelectItem<int?>> SelectItems = new();

    public int? SelectedValue {
        get
        {
            return Entity.GetProperty<int?>(PropertyInfo.Name, Nullable.GetUnderlyingType(PropertyInfo.Type!) != null ? null : 0);
        }
        set { Entity.SetProperty(PropertyInfo.Name, value, false); }
    }

    protected virtual List<SelectItem<int?>> GetSelectItemsFromEnum()
    {
        var selectItems = new List<SelectItem<int?>>();

        var isNullableType = Nullable.GetUnderlyingType(PropertyInfo.Type!) != null;
        var enumType = isNullableType
            ? Nullable.GetUnderlyingType(PropertyInfo.Type)!
            : PropertyInfo.Type;

        if (isNullableType)
        {
            selectItems.Add(new SelectItem<int?>());
        }
        foreach (var enumValue in enumType.GetEnumValues())
        {
            selectItems.Add(new SelectItem<int?>
            {
                Value = (int)enumValue,
                Text = AbpEnumLocalizer.GetString(enumType, enumValue, new []{ StringLocalizerFactory.CreateDefaultOrNull() })
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
            var isNullableType = Nullable.GetUnderlyingType(PropertyInfo.Type!) != null;
            if (!isNullableType)
            {
                var enumType = isNullableType
                    ? Nullable.GetUnderlyingType(PropertyInfo.Type)!
                    : PropertyInfo.Type;
                SelectedValue = (int)enumType.GetEnumValues().GetValue(0)!;
            }
        }
    }
}

public class SelectItem<TValue>
{
    public string Text { get; set; } = default!;
    public TValue Value { get; set; } = default!;
}
