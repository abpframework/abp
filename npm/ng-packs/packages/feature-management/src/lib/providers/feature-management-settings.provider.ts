import { SettingTabsService } from '@abp/ng.setting-management/config';
import { APP_INITIALIZER } from '@angular/core';
import { eFeatureManagementTabNames } from '../enums/feature-management-tab-names';
import { FeatureManagementTabComponent } from '../components';

export const FEATURE_MANAGEMENT_SETTINGS_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureSettingTabs,
    deps: [SettingTabsService],
    multi: true,
  },
];

export function configureSettingTabs(settingtabs: SettingTabsService) {
  return () => {
    settingtabs.add([
      {
        name: eFeatureManagementTabNames.FeatureManagement,
        order: 104,
        requiredPolicy: 'FeatureManagement.ManageHostFeatures',
        component: FeatureManagementTabComponent,
      },
    ]);
  };
}
