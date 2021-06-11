using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.Blazor.Components
{
    public partial class FeatureManagementModal
    {
        [Inject] protected IFeatureAppService FeatureAppService { get; set; }

        [Inject] protected IUiMessageService UiMessageService { get; set; }

        [Inject] protected IStringLocalizerFactory HtmlLocalizerFactory { get; set; }

        [Inject] protected IOptions<AbpLocalizationOptions> LocalizationOptions { get; set; }

        protected Modal Modal;

        protected string ProviderName;
        protected string ProviderKey;

        protected string SelectedTabName;

        protected List<FeatureGroupDto> Groups { get; set; }

        protected Dictionary<string, bool> ToggleValues;

        protected Dictionary<string, string> SelectionStringValues;

        public virtual async Task OpenAsync([NotNull]string providerName, string providerKey = null)
        {
            try
            {
                ProviderName = providerName;
                ProviderKey = providerKey;

                ToggleValues = new Dictionary<string, bool>();
                SelectionStringValues = new Dictionary<string, string>();

                Groups = (await FeatureAppService.GetAsync(ProviderName, ProviderKey)).Groups;

                SelectedTabName = GetNormalizedGroupName(Groups.First().Name);

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

                await InvokeAsync(Modal.Show);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        public virtual Task CloseModal()
        {
            return InvokeAsync(Modal.Hide);
        }

        protected virtual async Task SaveAsync()
        {
            try
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

                await InvokeAsync(Modal.Hide);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        protected virtual string GetNormalizedGroupName(string name)
        {
            return "FeatureGroup_" + name.Replace(".", "_");
        }

        protected virtual string GetFeatureStyles(FeatureDto feature)
        {
            return $"margin-left: {feature.Depth * 20}px";
        }

        protected virtual bool IsDisabled(string providerName)
        {
            return providerName != ProviderName && providerName != DefaultValueFeatureValueProvider.ProviderName;
        }

        protected virtual async Task OnFeatureValueChangedAsync(string value, FeatureDto feature)
        {
            if (feature?.ValueType?.Validator.IsValid(value) == true)
            {
                feature.Value = value;
            }
            else
            {
                await UiMessageService.Warn(L["Volo.Abp.FeatureManagement:InvalidFeatureValue", feature.DisplayName]);
            }
        }

        protected virtual Task OnSelectedValueChangedAsync(bool value, FeatureDto feature)
        {
            ToggleValues[feature.Name] = value;

            if (value)
            {
                CheckParents(feature.ParentName);
            }
            else
            {
                UncheckChildren(feature.Name);
            }

            return Task.CompletedTask;
        }

        protected virtual void CheckParents(string parentName)
        {
            if (parentName.IsNullOrWhiteSpace())
            {
                return;
            }

            foreach (var featureGroupDto in Groups)
            {
                foreach (var featureDto in featureGroupDto.Features)
                {
                    if (featureDto.Name == parentName && ToggleValues.ContainsKey(featureDto.Name))
                    {
                        ToggleValues[featureDto.Name] = true;
                        CheckParents(featureDto.ParentName);
                    }
                }
            }
        }

        protected virtual void UncheckChildren(string featureName)
        {
            foreach (var featureGroupDto in Groups)
            {
                foreach (var featureDto in featureGroupDto.Features)
                {
                    if (featureDto.ParentName == featureName && ToggleValues.ContainsKey(featureDto.Name))
                    {
                        ToggleValues[featureDto.Name] = false;
                        UncheckChildren(featureDto.Name);
                    }
                }
            }
        }

        protected virtual IStringLocalizer CreateStringLocalizer(string resourceName)
        {
            var resource = LocalizationOptions.Value.Resources.Values.FirstOrDefault(x => x.ResourceName == resourceName);
            return HtmlLocalizerFactory.Create(resource != null ? resource.ResourceType : LocalizationOptions.Value.DefaultResourceType);
        }

        protected virtual void ClosingModal( ModalClosingEventArgs eventArgs )
        {
            eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        }
    }
}
