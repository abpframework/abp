using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility;
using Volo.Abp.Data;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class LookupExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected List<SelectItem<object>> lookupItems = new();

    [Inject] public ILookupApiRequestService LookupApiService { get; set; } = default!;

    public string TextPropertyName => PropertyInfo.Name + "_Text";

    public object? SelectedValue {
        get { return Entity.GetProperty(PropertyInfo.Name); }
        set {
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
            lookupItems.Add(new SelectItem<object>
            {
                Text = Entity.GetProperty(TextPropertyName)!.ToString()!,
                Value = value
            });
        }
    }
    
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await SearchFilterChangedAsync(string.Empty);
        }
    }

    protected virtual void UpdateLookupTextProperty(object? value)
    {
        var selectedItemText = lookupItems.SingleOrDefault(t => t.Value.Equals(value))?.Text;
        Entity.SetProperty(TextPropertyName, selectedItemText);
    }

    protected virtual async Task<List<SelectItem<object>>> GetLookupItemsAsync(string filter)
    {
        var selectItems = new List<SelectItem<object>>();

        var url = PropertyInfo.Lookup.Url;
        if (!filter.IsNullOrEmpty())
        {
            url += $"?{PropertyInfo.Lookup.FilterParamName}={filter.Trim()}";
        }

        var response = await LookupApiService.SendAsync(url);

        var document = JsonDocument.Parse(response);
        var itemsArrayProp = document.RootElement.GetProperty(PropertyInfo.Lookup.ResultListPropertyName);
        foreach (var item in itemsArrayProp.EnumerateArray())
        {
            selectItems.Add(new SelectItem<object>
            {
                Text = item.GetProperty(PropertyInfo.Lookup.DisplayPropertyName).GetString()!,
                Value = JsonSerializer.Deserialize(item.GetProperty(PropertyInfo.Lookup.ValuePropertyName).GetRawText(), PropertyInfo.Type)!
            });
        }

        return selectItems;
    }

    protected virtual Task SelectedValueChanged(object selectedItem)
    {
        SelectedValue = selectedItem;
        return Task.CompletedTask;
    }

    protected virtual async Task SearchFilterChangedAsync(string filter)
    {
        lookupItems = await GetLookupItemsAsync(filter);
    }
}
