import { makeEnvironmentProviders } from '@angular/core';
import { CMS_KIT_ADMIN_ROUTE_PROVIDERS } from './route.provider';
import { CMS_KIT_ADMIN_SETTING_TAB_PROVIDERS } from './setting-tab.provider';

export function provideCmsKitAdminConfig() {
  return makeEnvironmentProviders([
    CMS_KIT_ADMIN_ROUTE_PROVIDERS,
    CMS_KIT_ADMIN_SETTING_TAB_PROVIDERS,
  ]);
}
