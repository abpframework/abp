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
using Volo.Abp.Data;
using Volo.Abp.Http;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending
{
    public partial class LookupExtensionProperty<TEntity, TResourceType> : ComponentBase
        where TEntity : IHasExtraProperties
    {
        protected List<SelectItem<object>> lookupItems;

        [Inject]
        public IStringLocalizerFactory StringLocalizerFactory { get; set; }

        [Parameter]
        public TEntity Entity { get; set; }

        [Parameter]
        public ObjectExtensionPropertyInfo PropertyInfo { get; set; }

        [Inject]
        public IRemoteServiceHttpClientAuthenticator ClientAuthenticator { get; set; }

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        [Inject]
        public ICurrentTenant CurrentTenant { get; set; }

        [Inject]
        public IOptions<AbpRemoteServiceOptions> RemoteServiceOptions { get; set; }

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
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!filter.IsNullOrEmpty())
            {
                if (uri.Query.IsNullOrEmpty())
                {
                    url += $"?{PropertyInfo.Lookup.FilterParamName}={filter.Trim()}";
                }
                else
                {
                    url += $"&{PropertyInfo.Lookup.FilterParamName}={filter.Trim()}";
                }
            }

            var client = HttpClientFactory.CreateClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeaders(requestMessage);

            if (!uri.IsAbsoluteUri)
            {
                var remoteServiceConfig = RemoteServiceOptions.Value.RemoteServices.GetConfigurationOrDefault("Default");
                client.BaseAddress = new Uri(remoteServiceConfig.BaseUrl);
                await ClientAuthenticator.Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, requestMessage, new RemoteServiceConfiguration(remoteServiceConfig.BaseUrl), string.Empty));
            }

            var response = await client.SendAsync(requestMessage);

            var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
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

        protected virtual void SelectedValueChanged(object selectedItem)
        {
            SelectedValue = selectedItem;
        }

        protected virtual async Task SearchFilterChangedAsync(string filter)
        {
            lookupItems = await GetLookupItemsAsync(filter);
        }

        protected virtual void AddHeaders(HttpRequestMessage requestMessage)
        {
            if (CurrentTenant.Id.HasValue)
            {
                requestMessage.Headers.Add(TenantResolverConsts.DefaultTenantKey, CurrentTenant.Id.Value.ToString());
            }

            var currentCulture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if (!currentCulture.IsNullOrEmpty())
            {
                requestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(currentCulture));
            }

            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeTypes.Application.Json));
        }
    }
}
