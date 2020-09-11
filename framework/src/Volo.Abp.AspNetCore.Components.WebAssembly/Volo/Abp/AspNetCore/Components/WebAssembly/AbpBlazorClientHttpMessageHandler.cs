using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class AbpBlazorClientHttpMessageHandler : DelegatingHandler, ITransientDependency
    {
        private readonly IJSRuntime _jsRuntime;

        public AbpBlazorClientHttpMessageHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await SetLanguageAsync(request, cancellationToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task SetLanguageAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var selectedLanguage = await _jsRuntime.InvokeAsync<string>(
                "localStorage.getItem",
                cancellationToken,
                "Abp.SelectedLanguage"
            );

            if (!selectedLanguage.IsNullOrWhiteSpace())
            {
                request.Headers.AcceptLanguage.Clear();
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(selectedLanguage));
            }
        }
    }
}
