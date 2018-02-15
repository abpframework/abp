using System.Collections.Generic;

namespace Volo.Abp.Permissions
{
    public class PermissionGrantInfoDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ParentName { get; set; }

        public bool IsGranted { get; set; }

        public List<ProviderInfoDto> Providers { get; set; }
    }
}