using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    [Serializable]
    public class ApplicationConfigurationDto
    {
        public ApplicationLocalizationConfigurationDto Localization { get; set; }

        public ApplicationAuthConfigurationDto Auth { get; set; }

        public ApplicationSettingConfigurationDto Setting { get; set; }

        public CurrentUserDto CurrentUser { get; set; }
    }
}