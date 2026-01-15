using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Data;

namespace Volo.Abp.MudBlazorUI.Components;

public partial class AbpMudExtensibleDataGrid<TItem> : ComponentBase
{
    protected const string DataFieldAttributeName = "Data";

    protected Regex ExtensionPropertiesRegex = new Regex(@"ExtraProperties\[(.*?)\]");

    [Parameter] public IEnumerable<TItem>? Data { get; set; }

    [Parameter] public Func<GridState<TItem>, Task<GridData<TItem>>>? ServerData { get; set; }

    [Parameter] public bool Loading { get; set; }

    [Parameter] public bool ShowPager { get; set; } = true;

    [Parameter] public int PageSize { get; set; } = 10;

    [Parameter] public IEnumerable<TableColumn> Columns { get; set; } = default!;

    [Parameter] public int CurrentPage { get; set; } = 1;

    [Parameter] public string? Class { get; set; }

    [Parameter] public string ActionColumnWidth { get; set; } = "150px";

    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; } = default!;

    [Inject]
    public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; } = default!;

    protected virtual RenderFragment RenderCustomTableColumnComponent(Type type, object data)
    {
        return (builder) =>
        {
            builder.OpenComponent(type);
            builder.AddAttribute(0, DataFieldAttributeName, data);
            builder.CloseComponent();
        };
    }

    protected virtual string GetConvertedFieldValue(TItem? item, TableColumn columnDefinition)
    {
        if (item == null)
        {
            return string.Empty;
        }

        var convertedValue = columnDefinition.ValueConverter!.Invoke(item!);
        if (!columnDefinition.DisplayFormat.IsNullOrEmpty())
        {
            return string.Format(columnDefinition.DisplayFormatProvider, columnDefinition.DisplayFormat!,
                convertedValue);
        }

        return convertedValue;
    }

    protected virtual object? GetPropertyValue(TItem? item, string propertyPath)
    {
        if (item == null)
        {
            return null;
        }

        var properties = propertyPath.Split('.');
        object? value = item;

        foreach (var prop in properties)
        {
            if (value == null)
            {
                return null;
            }

            var propertyInfo = value.GetType().GetProperty(prop, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                return null;
            }

            value = propertyInfo.GetValue(value);
        }

        return value;
    }

    protected virtual string GetColumnStyle(TableColumn column)
    {
        if (!string.IsNullOrEmpty(column.Width))
        {
            return $"width: {column.Width}";
        }

        return string.Empty;
    }

    protected virtual Func<TItem, object>? GetExtensionPropertySortFunc(TableColumn column)
    {
        if (!column.Sortable)
        {
            return null;
        }

        var propertyName = ExtensionPropertiesRegex.Match(column.Data).Groups[1].Value;
        return item =>
        {
            var entity = item as IHasExtraProperties;
            return entity?.GetProperty(propertyName) ?? string.Empty;
        };
    }

    protected virtual Color GetColor(object? color)
    {
        if (color == null)
        {
            return Color.Primary;
        }

        // Handle if it's already a MudBlazor Color
        if (color is Color mudColor)
        {
            return mudColor;
        }

        // Handle string color names
        if (color is string colorString)
        {
            return colorString.ToLowerInvariant() switch
            {
                "primary" => Color.Primary,
                "secondary" => Color.Secondary,
                "success" => Color.Success,
                "warning" => Color.Warning,
                "error" or "danger" => Color.Error,
                "info" => Color.Info,
                "dark" => Color.Dark,
                _ => Color.Primary
            };
        }

        return Color.Primary;
    }
}
