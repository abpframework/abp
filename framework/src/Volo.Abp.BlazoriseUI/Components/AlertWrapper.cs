using Volo.Abp.AspNetCore.Components.WebAssembly.Alerts;

namespace Volo.Abp.BlazoriseUI.Components
{
    internal class AlertWrapper
    {
        public AlertMessage AlertMessage { get; set; }
        public bool IsVisible { get; set; }
    }
}