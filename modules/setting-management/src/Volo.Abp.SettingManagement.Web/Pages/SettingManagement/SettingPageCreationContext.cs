using System.Collections.Generic;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement
{
    public class SettingPageCreationContext
    {
        public List<SettingPageGroup> Groups { get; }

        public SettingPageCreationContext()
        {
            Groups = new List<SettingPageGroup>();
        }
    }
}