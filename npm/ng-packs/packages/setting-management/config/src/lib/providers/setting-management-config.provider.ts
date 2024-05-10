import { makeEnvironmentProviders } from '@angular/core';
import { SETTING_MANAGEMENT_FEATURES_PROVIDERS } from './features.token';
import { SETTING_MANAGEMENT_ROUTE_PROVIDERS } from './route.provider';
import { SETTING_MANAGEMENT_SETTING_TAB_PROVIDERS } from './setting-tab.provider';
import { SETTING_MANAGEMENT_VISIBLE_PROVIDERS } from './visible.provider';

export function provideSettingManagementConfig() {
  return makeEnvironmentProviders([
    SETTING_MANAGEMENT_ROUTE_PROVIDERS,
    SETTING_MANAGEMENT_SETTING_TAB_PROVIDERS,
    SETTING_MANAGEMENT_FEATURES_PROVIDERS,
    SETTING_MANAGEMENT_VISIBLE_PROVIDERS,
  ]);
}
