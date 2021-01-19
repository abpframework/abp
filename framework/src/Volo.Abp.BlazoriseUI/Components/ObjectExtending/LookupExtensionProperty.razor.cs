using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending
{
    public partial class LookupExtensionProperty<TEntity, TResourceType> : ComponentBase
        where TEntity : IHasExtraProperties
    {
        protected List<SelectItem<object>> lookupItems;

        [Inject]
        public IStringLocalizerFactory StringLocalizerFactory { get; set; }

        [Inject]
        public HttpClient Client { get; set; }

        [Parameter]
        public TEntity Entity { get; set; }

        [Parameter]
        public ObjectExtensionPropertyInfo PropertyInfo { get; set; }

        public object SelectedValue
        {
            get
            {
                return Entity.GetProperty(PropertyInfo.Name);
            }
            set
            {
                Entity.SetProperty(PropertyInfo.Name, value);
                UpdateLookupTextProperty(value);
            }
        }


        public LookupExtensionProperty()
        {
            lookupItems = new List<SelectItem<object>>();
        }

        protected override async Task OnInitializedAsync()
        {
            lookupItems = await GetLookupItemsAsync(string.Empty);
        }

        protected virtual void UpdateLookupTextProperty(object value)
        {
            var lookupPropertyName = $"{PropertyInfo.Name}_Text";
            var selectedItemText = lookupItems.SingleOrDefault(t => t.Value.Equals(value)).Text;
            Entity.SetProperty(lookupPropertyName, selectedItemText);
        }

        protected virtual async Task<List<SelectItem<object>>> GetLookupItemsAsync(string filter)
        {
            var selectItems = new List<SelectItem<object>>();
            var url = PropertyInfo.Lookup.Url;
            if (!filter.IsNullOrEmpty())
            {
                url += $"{PropertyInfo.Lookup.FilterParamName}={filter.Trim()}";
            }

            var responseStream = await Client.GetStreamAsync(url);

            var document = await JsonDocument.ParseAsync(responseStream);
            var itemsArrayProp = document.RootElement.GetProperty(PropertyInfo.Lookup.ResultListPropertyName);
            foreach (var item in itemsArrayProp.EnumerateArray())
            {
                selectItems.Add(new SelectItem<object>
                {
                    Text = item.GetProperty(PropertyInfo.Lookup.DisplayPropertyName).GetString(),
                    Value = JsonSerializer.Deserialize(item.GetProperty(PropertyInfo.Lookup.ValuePropertyName).GetRawText(), PropertyInfo.Type)
                });
            }

            return selectItems;
        }

        protected virtual async Task SearchFilterChangedAsync(string filter)
        {
            lookupItems = await GetLookupItemsAsync(filter);
        }
    }
}
