using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Features;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement.Blazor.Components
{
    public partial class FeatureManagementModal
    {
        [Inject] private IFeatureAppService FeatureAppService { get; set; }

        private Modal _modal;
        
        private string _providerName;
        private string _providerKey;
        
        private List<FeatureGroupDto> _groups { get; set; }

        private Dictionary<string, bool> ToggleValues;
        
        public async Task OpenAsync(string providerName, string providerKey)
        {
            _providerName = providerName;
            _providerKey = providerKey;

            _groups = (await FeatureAppService.GetAsync(_providerName, _providerKey)).Groups;

            ToggleValues = _groups
                .SelectMany(x => x.Features)
                .Where(x => x.ValueType is ToggleStringValueType)
                .ToDictionary(x => x.Name, x => bool.Parse(x.Value));
            
            _modal.Show();
        }
        
        private void CloseModal()
        {
            _modal.Hide();
        }
        
        private async Task SaveAsync()
        {
            var features = new UpdateFeaturesDto
            {
                Features = _groups.SelectMany(g => g.Features).Select(f => new UpdateFeatureDto
                {
                    Name = f.Name,
                    Value = f.ValueType is ToggleStringValueType ? ToggleValues[f.Name].ToString() : f.Value
                }).ToList()
            };
            
            await FeatureAppService.UpdateAsync(_providerName, _providerKey, features);
            
            _modal.Hide();
        }
        
        public string GetNormalizedGroupName(string name)
        {
            return "FeatureGroup_" + name.Replace(".", "_");
        }
        
        public virtual bool IsDisabled(string providerName)
        {
            return providerName != _providerName && providerName != DefaultValueFeatureValueProvider.ProviderName;
        }
    }
}