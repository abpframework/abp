using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Data;
using Volo.Abp.Timing;

namespace Volo.Abp.MudBlazorUI.Components;

public partial class AbpMudExtensibleDataGrid<TItem> : ComponentBase
{
    protected const string DataFieldAttributeName = "Data";

    private MudDataGrid<TItem>? _dataGrid;

    protected Regex ExtensionPropertiesRegex = new Regex(@"ExtraProperties\[(.*?)\]");

    /// <summary>
    /// Maps SortBy func references to their corresponding property names.
    /// Used to resolve property names from MudBlazor SortDefinition.SortFunc.
    /// </summary>
    private readonly Dictionary<Func<TItem, object?>, string> _sortFuncToPropertyMap = new();

    [Parameter] public Func<GridState<TItem>, CancellationToken, Task<GridData<TItem>>>? ServerData { get; set; }

    [Parameter] public bool Loading { get; set; }

    [Parameter] public bool ShowPager { get; set; } = true;

    [Parameter] public int PageSize { get; set; } = 10;

    [Parameter] public IEnumerable<TableColumn> Columns { get; set; } = default!;

    [Parameter] public int CurrentPage { get; set; } = 1;

    [Parameter] public string? Class { get; set; }

    [Parameter] public string ActionColumnWidth { get; set; } = "150px";

    protected int ZeroBasedCurrentPage => CurrentPage > 0 ? CurrentPage - 1 : 0;

    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; } = default!;

    [Inject]
    public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; } = default!;

    [Inject]
    public IClock Clock { get; set; } = default!;

    public virtual async Task ReloadServerDataAsync()
    {
        if (_dataGrid != null && ServerData != null)
        {
            await InvokeAsync(() => _dataGrid.ReloadServerData());
        }
    }

    /// <summary>
    /// Resolves the property name for a given SortDefinition by matching its SortFunc reference.
    /// When MudBlazor uses Func for SortBy on TemplateColumn, it returns an internal GUID identifier
    /// instead of the property name. This method looks up the original property name from the
    /// registered sort function mapping.
    /// </summary>
    /// <returns>The property name if found in the mapping, otherwise falls back to SortDefinition.SortBy.</returns>
    public virtual string ResolveSortPropertyName(SortDefinition<TItem> sortDefinition)
    {
        if (sortDefinition.SortFunc != null && _sortFuncToPropertyMap.TryGetValue(sortDefinition.SortFunc, out var propertyName))
        {
            return propertyName;
        }

        return sortDefinition.SortBy;
    }

    protected virtual RenderFragment RenderCustomTableColumnComponent(Type type, object data)
    {
        return (builder) =>
        {
            builder.OpenComponent(0, type);
            builder.AddAttribute(1, DataFieldAttributeName, data);
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

    protected virtual string GetConvertedFieldValue(object? value, TableColumn columnDefinition)
    {
        if (value is DateTime dateTime)
        {
            var converted = Clock.ConvertToUserTime(dateTime);
            if (!columnDefinition.DisplayFormat.IsNullOrEmpty())
            {
                return string.Format(columnDefinition.DisplayFormatProvider, columnDefinition.DisplayFormat!, converted);
            }

            return converted.ToString(columnDefinition.DisplayFormatProvider as CultureInfo ?? CultureInfo.CurrentCulture);
        }

        if (value is DateTimeOffset dateTimeOffset)
        {
            var converted = Clock.ConvertToUserTime(dateTimeOffset);
            if (!columnDefinition.DisplayFormat.IsNullOrEmpty())
            {
                return string.Format(columnDefinition.DisplayFormatProvider, columnDefinition.DisplayFormat!, converted);
            }

            return converted.ToString(columnDefinition.DisplayFormatProvider as CultureInfo ?? CultureInfo.CurrentCulture);
        }

        if (value == null)
        {
            return string.Empty;
        }

        if (!columnDefinition.DisplayFormat.IsNullOrEmpty())
        {
            return string.Format(columnDefinition.DisplayFormatProvider, columnDefinition.DisplayFormat!, value);
        }

        return value.ToString() ?? string.Empty;
    }

    protected virtual object GetPropertyValue(TItem item, string propertyPath)
    {
        ArgumentNullException.ThrowIfNull(item);

        var properties = propertyPath.Split('.');
        object value = item;

        foreach (var prop in properties)
        {
            var propertyInfo = value.GetType().GetProperty(prop, BindingFlags.Public | BindingFlags.Instance)
                               ?? throw new ArgumentException($"Property '{prop}' not found on type '{value.GetType().Name}'", nameof(propertyPath));

            var propertyValue = propertyInfo.GetValue(value);

            // Allow null property values and treat them as empty for display/sorting purposes
            if (propertyValue == null)
            {
                return string.Empty;
            }

            value = propertyValue;
        }

        if (value is DateTime dateTime)
        {
            return Clock.ConvertToUserTime(dateTime);
        }

        if (value is DateTimeOffset dateTimeOffset)
        {
            return Clock.ConvertToUserTime(dateTimeOffset);
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

    protected virtual Func<TItem, object?>? GetSortByFunc(TableColumn column)
    {
        if (!column.Sortable)
        {
            return null;
        }

        // Determine the property path for sorting
        string? propertyPath = null;
        if (!string.IsNullOrEmpty(column.PropertyName))
        {
            propertyPath = column.PropertyName;
        }
        else if (!string.IsNullOrEmpty(column.Data) && !ExtensionPropertiesRegex.IsMatch(column.Data))
        {
            propertyPath = column.Data;
        }

        if (string.IsNullOrEmpty(propertyPath))
        {
            return null;
        }

        var properties = propertyPath.Split('.');

        // Build a lambda expression that directly accesses the property
        var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TItem), "x");
        System.Linq.Expressions.Expression expression = parameter;

        Func<TItem, object?>? sortFunc = null;
        foreach (var prop in properties)
        {
            var propertyInfo = expression.Type.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                // Fallback to GetPropertyValue if property not found
                sortFunc = item => GetPropertyValue(item, propertyPath);
                break;
            }
            expression = System.Linq.Expressions.Expression.Property(expression, propertyInfo);
        }

        if (sortFunc == null)
        {
            var convertExpression = System.Linq.Expressions.Expression.Convert(expression, typeof(object));
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TItem, object?>>(convertExpression, parameter);
            sortFunc = lambda.Compile();
        }

        // Register the mapping so we can resolve property name from SortDefinition.SortFunc later
        _sortFuncToPropertyMap[sortFunc] = propertyPath;

        return sortFunc;
    }

    protected virtual Func<TItem, object?>? GetExtensionPropertySortFunc(TableColumn column)
    {
        if (!column.Sortable || string.IsNullOrWhiteSpace(column.Data))
        {
            return null;
        }

        var match = ExtensionPropertiesRegex.Match(column.Data);
        if (!match.Success)
        {
            return null;
        }

        var propertyName = match.Groups[1].Value;
        Func<TItem, object?> sortFunc = item =>
        {
            var entity = item as IHasExtraProperties;
            return entity?.GetProperty(propertyName) ?? string.Empty;
        };

        // Register the mapping so we can resolve property name from SortDefinition.SortFunc later
        _sortFuncToPropertyMap[sortFunc] = propertyName;

        return sortFunc;
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
