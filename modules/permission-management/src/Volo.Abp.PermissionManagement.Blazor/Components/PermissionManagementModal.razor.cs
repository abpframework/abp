using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.PermissionManagement.Blazor.Components
{
    public partial class PermissionManagementModal
    {
        [Inject] private IPermissionAppService PermissionAppService { get; set; }

        private Modal _modal;
        
        private string _providerName;
        private string _providerKey;

        private string _entityDisplayName;
        private List<PermissionGroupDto> _groups;
        
        public async Task OpenAsync(string providerName, string providerKey)
        {
            _providerName = providerName;
            _providerKey = providerKey;

            var result = await PermissionAppService.GetAsync(_providerName, _providerKey);

            _entityDisplayName = result.EntityDisplayName;
            _groups = result.Groups;

            _modal.Show();
        }

        private void CloseModal()
        {
            _modal.Hide();
        }

        private async Task SaveAsync()
        {
            
        }

        public string GetNormalizedGroupName(string name)
        {
            return "PermissionGroup_" + name.Replace(".", "_");
        }
    }
}
