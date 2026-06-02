using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.FeatureManagement.Blazor.MudBlazor;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.Blazor.MudBlazor.Components;

public partial class FeatureManagementModal
{
    [Inject] protected IFeatureAppService FeatureAppService { get; set; } = default!;

    [Inject] protected IUiMessageService UiMessageService { get; set; } = default!;

    [Inject] protected IStringLocalizerFactory HtmlLocalizerFactory { get; set; } = default!;

    [Inject] protected IOptions<AbpLocalizationOptions> LocalizationOptions { get; set; } = default!;

    [Inject] protected ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; } = default!;

    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Inject] protected IDialogService DialogService { get; set; } = default!;

    protected bool _isVisible;
    protected int _activeTabIndex = 0;

    protected string? ProviderName;
    protected string? ProviderKey;
    protected string? ProviderKeyDisplayName;

    protected List<FeatureGroupDto>? Groups { get; set; }

    protected Dictionary<string, bool> ToggleValues = new();

    protected Dictionary<string, string> SelectionStringValues = new();

    public virtual async Task OpenAsync(string providerName, string? providerKey = null, string? providerKeyDisplayName = null)
    {
        try
        {
            ProviderName = providerName;
            ProviderKey = providerKey;

            ToggleValues = new Dictionary<string, bool>();
            SelectionStringValues = new Dictionary<string, string>();

            if (!providerKeyDisplayName.IsNullOrWhiteSpace())
            {
                ProviderKeyDisplayName = $" - {providerKeyDisplayName}";
            }
            else
            {
                ProviderKeyDisplayName = string.Empty;
            }

            var result = await FeatureAppService.GetAsync(ProviderName, ProviderKey);

            Groups = result?.Groups ?? new List<FeatureGroupDto>();

            if (Groups.Any())
            {
                _activeTabIndex = 0;
            }

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

            _isVisible = true;
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    public virtual Task CloseModal()
    {
        _isVisible = false;
        return InvokeAsync(StateHasChanged);
    }

    protected virtual async Task SaveAsync()
    {
        try
        {
            if (Groups == null)
            {
                return;
            }

            var features = new UpdateFeaturesDto
            {
                Features = Groups.SelectMany(g => g.Features).Select(f => new UpdateFeatureDto
                {
                    Name = f.Name,
                    Value = f.ValueType is ToggleStringValueType ? ToggleValues[f.Name].ToString() :
                        f.ValueType is SelectionStringValueType ? SelectionStringValues[f.Name] : f.Value
                }).ToList()
            };

            await FeatureAppService.UpdateAsync(ProviderName!, ProviderKey, features);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            _isVisible = false;
            await InvokeAsync(StateHasChanged);
            Snackbar.Add(L["SavedSuccessfully"], Severity.Success);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    public virtual async Task DeleteAsync(string providerName, string? providerKey = null)
    {
        try
        {
            var confirmed = await DialogService.ShowMessageBoxAsync(
                L["AreYouSure"],
                L["AreYouSureToResetToDefault"],
                yesText: L["Yes"],
                cancelText: L["Cancel"]);

            if (confirmed != true)
            {
                return;
            }

            await FeatureAppService.DeleteAsync(ProviderName!, ProviderKey);
            Snackbar.Add(L["ResetedToDefault"], Severity.Success);

            await CloseModal();
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
        return $"margin-inline-start: {feature.Depth * 20}px";
    }

    protected virtual bool IsDisabled(FeatureDto feature)
    {
        return feature.Value != null &&
               feature.Provider.Name != null &&
               feature.Provider.Name != ProviderName &&
               feature.Provider.Name != DefaultValueFeatureValueProvider.ProviderName;
    }

    public virtual string GetShownName(FeatureDto featureDto)
    {
        return !IsDisabled(featureDto)
            ? featureDto.DisplayName
            : $"{featureDto.DisplayName} ({featureDto.Provider.Name})";
    }

    protected virtual async Task OnFeatureValueChangedAsync(string? value, FeatureDto feature)
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

    protected virtual void CheckParents(string? parentName)
    {
        if (parentName.IsNullOrWhiteSpace() || Groups == null)
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
        if (Groups == null)
        {
            return;
        }

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
        return HtmlLocalizerFactory.CreateByResourceNameOrNull(resourceName) ??
               HtmlLocalizerFactory.CreateDefaultOrNull();
    }
}
