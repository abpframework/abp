using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.Web.Pages.FeatureManagement
{
    public class FeatureManagementModal : AbpPageModel
    {
        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ProviderName { get; set; }

        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ProviderKey { get; set; }

        [BindProperty]
        public List<FeatureViewModel> Features { get; set; }

        public FeatureListDto FeatureListDto { get; set; }

        private readonly IFeatureAppService _featureAppService;

        public FeatureManagementModal(IFeatureAppService featureAppService)
        {
            ObjectMapperContext = typeof(AbpFeatureManagementWebModule);

            _featureAppService = featureAppService;
        }

        public async Task OnGetAsync()
        {
            FeatureListDto = await _featureAppService.GetAsync(ProviderName, ProviderKey);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var features = new UpdateFeaturesDto
            {
                Features = Features.Select(f => new UpdateFeatureDto
                {
                    Name = f.Name,
                    Value = f.Type == nameof(ToggleStringValueType) ? f.BoolValue.ToString() : f.Value
                }).ToList()
            };

            await _featureAppService.UpdateAsync(ProviderName, ProviderKey, features);

            return NoContent();
        }


        public class ProviderInfoViewModel
        {
            public string ProviderName { get; set; }

            public string ProviderKey { get; set; }
        }

        public class FeatureViewModel
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public bool BoolValue { get; set; }

            public string Type { get; set; }
        }
    }
}