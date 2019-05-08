using System.Collections.Generic;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement
{
    public class SettingManagementPageOptions
    {
        public List<ISettingPageContributor> Contributors { get; }

        public SettingManagementPageOptions()
        {
            Contributors = new List<ISettingPageContributor>();
        }
    }
}
