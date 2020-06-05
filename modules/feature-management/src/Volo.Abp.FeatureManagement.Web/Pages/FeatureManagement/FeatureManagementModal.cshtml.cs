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

        protected IFeatureAppService FeatureAppService { get; }

        public FeatureManagementModal(IFeatureAppService featureAppService)
        {
            ObjectMapperContext = typeof(AbpFeatureManagementWebModule);

            FeatureAppService = featureAppService;
        }

        public virtual async Task OnGetAsync()
        {
            FeatureListDto = await FeatureAppService.GetAsync(ProviderName, ProviderKey);
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var features = new UpdateFeaturesDto
            {
                Features = Features.Select(f => new UpdateFeatureDto
                {
                    Name = f.Name,
                    Value = f.Type == nameof(ToggleStringValueType) ? f.BoolValue.ToString() : f.Value
                }).ToList()
            };

            await FeatureAppService.UpdateAsync(ProviderName, ProviderKey, features);

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