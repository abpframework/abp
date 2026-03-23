import { ABP, ConfigStateService } from '@abp/ng.core';
import { SettingTabsService } from '@abp/ng.setting-management/config';
import { inject, provideAppInitializer } from '@angular/core';
import { eCmsKitAdminPolicyNames, eCmsKitAdminRouteNames } from '../enums';
import { CmsSettingsComponent } from '@abp/ng.cms-kit/admin';

export const CMS_KIT_ADMIN_SETTING_TAB_PROVIDERS = [
  provideAppInitializer(() => {
    configureSettingTabs();
  }),
];

export async function configureSettingTabs() {
  const settingTabs = inject(SettingTabsService);
  const configState = inject(ConfigStateService);
  const tabsArray: ABP.Tab[] = [
    {
      name: eCmsKitAdminRouteNames.Cms,
      order: 100,
      requiredPolicy: eCmsKitAdminPolicyNames.Cms,
      invisible: configState.getFeature('CmsKit.CommentEnable')?.toLowerCase() === 'true',
      component: CmsSettingsComponent,
    },
  ];

  settingTabs.add(tabsArray);
}
