import { SettingTabsService } from '@abp/ng.setting-management/config';
import { inject, provideAppInitializer } from '@angular/core';
import { eFeatureManagementTabNames } from '../enums/feature-management-tab-names';
import { FeatureManagementTabComponent } from '../components';

export const FEATURE_MANAGEMENT_SETTINGS_PROVIDERS = [
  provideAppInitializer(() => {
    configureSettingTabs();
  }),
];

export function configureSettingTabs() {
  const settingtabs = inject(SettingTabsService);
  settingtabs.add([
    {
      name: eFeatureManagementTabNames.FeatureManagement,
      order: 100,
      requiredPolicy: 'FeatureManagement.ManageHostFeatures',
      component: FeatureManagementTabComponent,
    },
  ]);
}
