using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.Blazor.Components
{
    public partial class FeatureManagementModal
    {
        [Inject] protected IFeatureAppService FeatureAppService { get; set; }
        
        [Inject] protected IUiMessageService UiMessageService { get; set; }
        
        [Inject] protected IStringLocalizer<AbpFeatureManagementResource> L { get; set; }
        
        [Inject] protected IStringLocalizerFactory HtmlLocalizerFactory { get; set; }
        
        [Inject] protected IOptions<AbpLocalizationOptions> LocalizationOptions { get; set; }

        protected Modal Modal;
        
        protected string ProviderName;
        protected string ProviderKey;
        
        protected List<FeatureGroupDto> Groups { get; set; }

        protected Dictionary<string, bool> ToggleValues;

        protected Dictionary<string, string> SelectionStringValues;
        
        public virtual async Task OpenAsync(string providerName, string providerKey)
        {
            ProviderName = providerName;
            ProviderKey = providerKey;

            ToggleValues = new Dictionary<string, bool>();
            SelectionStringValues = new Dictionary<string, string>();
            
            Groups = (await FeatureAppService.GetAsync(ProviderName, ProviderKey)).Groups;

            foreach (var featureGroupDto in Groups)
            {
                foreach (var featureDto in featureGroupDto.Features)
                {
                    if (featureDto.ValueType is ToggleStringValueType)
                    {
                        ToggleValues.Add(featureDto.Name, bool.Parse(featureDto.Value));
                    }

                    if (featureDto.ValueType is SelectionStringValueType)
                    {
                        SelectionStringValues.Add(featureDto.Name, featureDto.Value);
                    }
                }
            }

            Modal.Show();
        }
        
        public virtual Task CloseModal()
        {
            Modal.Hide();
            return Task.CompletedTask;
        }
        
        protected virtual async Task SaveAsync()
        {
            var features = new UpdateFeaturesDto
            {
                Features = Groups.SelectMany(g => g.Features).Select(f => new UpdateFeatureDto
                {
                    Name = f.Name,
                    Value = f.ValueType is ToggleStringValueType ? ToggleValues[f.Name].ToString() : 
                            f.ValueType is SelectionStringValueType ? SelectionStringValues[f.Name] : f.Value
                }).ToList()
            };
            
            await FeatureAppService.UpdateAsync(ProviderName, ProviderKey, features);
            
            Modal.Hide();
        }
        
        protected virtual string GetNormalizedGroupName(string name)
        {
            return "FeatureGroup_" + name.Replace(".", "_");
        }
        
        protected virtual bool IsDisabled(string providerName)
        {
            return providerName != ProviderName && providerName != DefaultValueFeatureValueProvider.ProviderName;
        }

        protected virtual async Task OnFeatureValueChangedAsync(string value, FeatureDto feature)
        {
            if (feature.ValueType.Validator.IsValid(value))
            {
                feature.Value = value;
            }
            else
            {
                await UiMessageService.WarnAsync(L["Volo.Abp.FeatureManagement:InvalidFeatureValue", feature.DisplayName]);
            }
        }

        protected virtual void SelectedValueChanged(string featureName, string value)
        {
            SelectionStringValues[featureName] = value;
        }
        
        protected virtual IStringLocalizer CreateStringLocalizer(string resourceName)
        {
            var resource = LocalizationOptions.Value.Resources.Values.FirstOrDefault(x => x.ResourceName == resourceName);
            return HtmlLocalizerFactory.Create(resource != null ? resource.ResourceType : LocalizationOptions.Value.DefaultResourceType);
        }
    }
}