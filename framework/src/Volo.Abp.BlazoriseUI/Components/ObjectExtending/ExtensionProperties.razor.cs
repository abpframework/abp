using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Data;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending
{
    public partial class ExtensionProperties<TEntityType, TResourceType> : ComponentBase
        where TEntityType : IHasExtraProperties
    {
        [Inject]
        public IStringLocalizerFactory StringLocalizerFactory { get; set; }

        [Parameter]
        public AbpBlazorMessageLocalizerHelper<TResourceType> LH { get; set; }

        [Parameter]
        public TEntityType Entity { get; set; }

        protected void TextChanged(string value, string propertyName)
        {
            Entity.SetProperty(propertyName, value);
        }

        protected void CheckedChanged(bool value, string propertyName)
        {
            Entity.SetProperty(propertyName, value);
        }

        protected void TimeChanged(TimeSpan? value, string propertyName)
        {
            Entity.SetProperty(propertyName, value);
        }

        protected void DateTimeChanged(DateTime? value, string propertyName)
        {
            Entity.SetProperty(propertyName, value);
        }
    }
}
