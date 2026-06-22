using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Blazorise.DataGrid;
using Blazorise.Extensions;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Timing;

namespace Volo.Abp.BlazoriseUI.Components;

public partial class AbpExtensibleDataGrid<TItem> : ComponentBase
{
    protected const string DataFieldAttributeName = "Data";

    protected Dictionary<string, DataGridEntityActionsColumn<TItem>> ActionColumns =
        new Dictionary<string, DataGridEntityActionsColumn<TItem>>();

    protected Regex ExtensionPropertiesRegex = new Regex(@"ExtraProperties\[(.*?)\]");

    [Parameter] public IEnumerable<TItem> Data { get; set; } = default!;

    [Parameter] public EventCallback<DataGridReadDataEventArgs<TItem>> ReadData { get; set; }

    [Parameter] public int? TotalItems { get; set; }

    [Parameter] public bool ShowPager { get; set; }

    [Parameter] public int PageSize { get; set; }

    [Parameter] public IEnumerable<TableColumn> Columns { get; set; } = default!;

    [Parameter] public int CurrentPage { get; set; } = 1;

    [Parameter] public string? Class { get; set; }

    [Parameter] public bool Responsive { get; set; }

    [Parameter] public bool AutoGenerateColumns { get; set; }

    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; } = default!;

    [Inject]
    public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; } = default!;

    [Inject]
    public IClock Clock { get; set; } = default!;

    protected virtual RenderFragment RenderCustomTableColumnComponent(Type type, object data)
    {
        return (builder) =>
        {
            builder.OpenComponent(type);
            builder.AddAttribute(0, DataFieldAttributeName, data);
            builder.CloseComponent();
        };
    }

    protected virtual string GetConvertedFieldValue(TItem item, TableColumn columnDefinition)
    {
        if (columnDefinition.ValueConverter != null)
        {
            var convertedValue = columnDefinition.ValueConverter.Invoke(item!);
            if (!columnDefinition.DisplayFormat.IsNullOrEmpty())
            {
                return string.Format(columnDefinition.DisplayFormatProvider, columnDefinition.DisplayFormat!, convertedValue);
            }

            return convertedValue;
        }

        var propertyInfo = item!.GetType().GetProperty(columnDefinition.Data);
        return GetConvertedFieldValue(propertyInfo?.GetValue(item), columnDefinition);
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

    protected virtual bool IsDateTimeColumn(TableColumn columnDefinition)
    {
        var propertyInfo = typeof(TItem).GetProperty(columnDefinition.Data);
        if (propertyInfo == null)
        {
            return false;
        }

        var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
        return propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset);
    }
}
