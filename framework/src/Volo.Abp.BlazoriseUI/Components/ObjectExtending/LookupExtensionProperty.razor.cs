using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility;
using Volo.Abp.Data;
using Volo.Abp.Http;
using Volo.Abp.Http.Client;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class LookupExtensionProperty<TEntity, TResourceType> : ComponentBase
    where TEntity : IHasExtraProperties
{
    protected List<SelectItem<object>> lookupItems;

    [Inject] public IStringLocalizerFactory StringLocalizerFactory { get; set; }

    [Parameter] public TEntity Entity { get; set; }

    [Parameter] public ObjectExtensionPropertyInfo PropertyInfo { get; set; }


    [Inject] public ILookupApiRequestService LookupApiService { get; set; }

    public string TextPropertyName => PropertyInfo.Name + "_Text";

    public object SelectedValue
    {
        get { return Entity.GetProperty(PropertyInfo.Name); }
        set
        {
            Entity.SetProperty(PropertyInfo.Name, value, false);
            UpdateLookupTextProperty(value);
        }
    }

    public LookupExtensionProperty()
    {
        lookupItems = new List<SelectItem<object>>();
    }

    protected override void OnParametersSet()
    {
        var value = Entity.GetProperty(PropertyInfo.Name);
        var text = Entity.GetProperty(TextPropertyName);
        if (value != null && text != null)
        {
            lookupItems.Add(new SelectItem<object>
            {
                Text = Entity.GetProperty(TextPropertyName).ToString(),
                Value = value
            });
        }
    }

    protected virtual void UpdateLookupTextProperty(object value)
    {
        var selectedItemText = lookupItems.SingleOrDefault(t => t.Value.Equals(value)).Text;
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
                Text = item.GetProperty(PropertyInfo.Lookup.DisplayPropertyName).GetString(),
                Value = JsonSerializer.Deserialize(
                    item.GetProperty(PropertyInfo.Lookup.ValuePropertyName).GetRawText(), PropertyInfo.Type)
            });
        }

        return selectItems;
    }

    protected virtual Task SelectedValueChanged(object selectedItem)
    {
        SelectedValue = selectedItem;

        return Task.CompletedTask;
    }

    protected async Task SearchFilterChangedAsync(string filter)
    {
        lookupItems = await GetLookupItemsAsync(filter);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await SearchFilterChangedAsync(string.Empty);
        }
    }
}
