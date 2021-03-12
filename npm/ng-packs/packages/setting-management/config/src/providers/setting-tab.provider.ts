import { SettingTabsService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { EmailSettingGroupComponent } from '../components/email-setting-group/email-setting-group.component';
import { eSettingManamagementSettingTabNames } from '../enums/setting-tab-names';

export const SETTING_MANAGEMENT_SETTING_TAB_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureSettingTabs,
    deps: [SettingTabsService],
    multi: true,
  },
];

export function configureSettingTabs(settingTabs: SettingTabsService) {
  return () => {
    settingTabs.add([
      {
        name: eSettingManamagementSettingTabNames.EmailSettingGroup,
        order: 100,
        requiredPolicy: 'SettingManagement.Emailing',
        component: EmailSettingGroupComponent,
      },
    ]);
  };
}
