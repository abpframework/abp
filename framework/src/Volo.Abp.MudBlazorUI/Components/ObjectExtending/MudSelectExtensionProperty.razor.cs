using System;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using Volo.Abp.Data;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public partial class MudSelectExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected List<MudSelectItem<int?>> SelectItems = new();

    public int? SelectedValue
    {
        get => Entity.GetProperty<int?>(PropertyInfo.Name, Nullable.GetUnderlyingType(PropertyInfo.Type!) != null ? null : 0);
        set => Entity.SetProperty(PropertyInfo.Name, value, false);
    }

    protected virtual List<MudSelectItem<int?>> GetSelectItemsFromEnum()
    {
        var selectItems = new List<MudSelectItem<int?>>();

        var isNullableType = Nullable.GetUnderlyingType(PropertyInfo.Type!) != null;
        var enumType = isNullableType
            ? Nullable.GetUnderlyingType(PropertyInfo.Type)!
            : PropertyInfo.Type;

        if (isNullableType)
        {
            selectItems.Add(new MudSelectItem<int?>());
        }

        foreach (var enumValue in enumType.GetEnumValues())
        {
            selectItems.Add(new MudSelectItem<int?>
            {
                Value = (int)enumValue,
                Text = AbpEnumLocalizer.GetString(enumType, enumValue, new[] { StringLocalizerFactory.CreateDefaultOrNull() })
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
                SelectedValue = (int)PropertyInfo.Type.GetEnumValues().GetValue(0)!;
            }
        }
    }
}

public class MudSelectItem<TValue>
{
    public string Text { get; set; } = default!;
    public TValue Value { get; set; } = default!;
}
