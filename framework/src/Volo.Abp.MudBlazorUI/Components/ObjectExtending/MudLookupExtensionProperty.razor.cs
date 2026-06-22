using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility;
using Volo.Abp.Data;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public partial class MudLookupExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected List<MudSelectItem<object>> LookupItems = new();

    [Inject]
    public ILookupApiRequestService LookupApiService { get; set; } = default!;

    public string TextPropertyName => PropertyInfo.Name + "_Text";

    private MudSelectItem<object>? _selectedItem;

    public MudSelectItem<object> SelectedItem {
        get => _selectedItem!;
        set => _selectedItem = value;
    }

    public object? SelectedValue
    {
        get => Entity.GetProperty(PropertyInfo.Name);
        set
        {
            Entity.SetProperty(PropertyInfo.Name, value, false);
            UpdateLookupTextProperty(value);
        }
    }

    protected override void OnParametersSet()
    {
        var value = Entity.GetProperty(PropertyInfo.Name);
        var text = Entity.GetProperty(TextPropertyName);
        if (value != null && text != null)
        {
            SelectedItem = new MudSelectItem<object>
            {
                Text = text.ToString()!,
                Value = value
            };
            LookupItems = new List<MudSelectItem<object>> { SelectedItem };
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            LookupItems = await GetLookupItemsAsync(string.Empty);
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual void UpdateLookupTextProperty(object? value)
    {
        var selectedItemText = LookupItems.SingleOrDefault(t => t.Value?.Equals(value) == true)?.Text;
        Entity.SetProperty(TextPropertyName, selectedItemText);
    }

    protected virtual async Task<List<MudSelectItem<object>>> GetLookupItemsAsync(string filter)
    {
        var selectItems = new List<MudSelectItem<object>>();

        var url = PropertyInfo.Lookup.Url;
        if (!string.IsNullOrEmpty(filter))
        {
            url += $"?{PropertyInfo.Lookup.FilterParamName}={filter.Trim()}";
        }

        var response = await LookupApiService.SendAsync(url);

        var document = JsonDocument.Parse(response);
        var itemsArrayProp = document.RootElement.GetProperty(PropertyInfo.Lookup.ResultListPropertyName);
        foreach (var item in itemsArrayProp.EnumerateArray())
        {
            selectItems.Add(new MudSelectItem<object>
            {
                Text = item.GetProperty(PropertyInfo.Lookup.DisplayPropertyName).GetString()!,
                Value = JsonSerializer.Deserialize(item.GetProperty(PropertyInfo.Lookup.ValuePropertyName).GetRawText(), PropertyInfo.Type)!
            });
        }

        return selectItems;
    }

    protected virtual Task SelectedItemChanged(MudSelectItem<object> item)
    {
        SelectedItem = item;
        SelectedValue = item?.Value;
        return Task.CompletedTask;
    }

    protected virtual async Task<IEnumerable<MudSelectItem<object>?>> SearchAsync(string? value, CancellationToken token)
    {
        LookupItems = await GetLookupItemsAsync(value ?? string.Empty);
        return LookupItems!;
    }
}
