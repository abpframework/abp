using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.Web.Pages.FeatureManagement;

public class FeatureManagementModal : AbpPageModel
{
    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ProviderName { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ProviderKey { get; set; }
    
    [HiddenInput]
    [BindProperty(SupportsGet = true)] 
    public string ProviderKeyDisplayName { get; set; }

    [BindProperty]
    public List<FeatureGroupViewModel> FeatureGroups { get; set; }

    public GetFeatureListResultDto FeatureListResultDto { get; set; }

    protected IFeatureAppService FeatureAppService { get; }

    protected ILocalEventBus LocalEventBus { get; }

    public AbpLocalizationOptions LocalizationOptions { get; }

    public FeatureManagementModal(
        IFeatureAppService featureAppService,
        ILocalEventBus localEventBus,
        IOptions<AbpLocalizationOptions> localizationOptions)
    {
        ObjectMapperContext = typeof(AbpFeatureManagementWebModule);

        FeatureAppService = featureAppService;
        LocalEventBus = localEventBus;
        LocalizationOptions = localizationOptions.Value;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ValidateModel();
        if (!ProviderKeyDisplayName.IsNullOrWhiteSpace())
        {
            ProviderKeyDisplayName = " - " + HttpUtility.HtmlEncode(ProviderKeyDisplayName);
        }
        FeatureListResultDto = await FeatureAppService.GetAsync(ProviderName, ProviderKey);

        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var features = new UpdateFeaturesDto
        {
            Features = FeatureGroups.SelectMany(g => g.Features).Select(f => new UpdateFeatureDto
            {
                Name = f.Name,
                Value = f.Type == nameof(ToggleStringValueType) ? f.BoolValue.ToString() : f.Value
            }).ToList()
        };

        await FeatureAppService.UpdateAsync(ProviderName, ProviderKey, features);

        await LocalEventBus.PublishAsync(
            new CurrentApplicationConfigurationCacheResetEventData()
        );

        return NoContent();
    }

    public class FeatureGroupViewModel
    {
        public List<FeatureViewModel> Features { get; set; }
    }

    public class FeatureViewModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool BoolValue { get; set; }

        public string Type { get; set; }
    }
}
