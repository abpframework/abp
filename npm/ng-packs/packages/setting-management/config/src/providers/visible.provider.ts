import { APP_INITIALIZER, Injector } from '@angular/core';
import { combineLatest } from 'rxjs';
import { RoutesService } from '@abp/ng.core';
import { SETTING_MANAGEMENT_HAS_SETTING } from './route.provider';
import { SETTING_MANAGEMENT_ROUTE_VISIBILITY } from './features.token';
import { eSettingManagementRouteNames } from '../enums';

export const SETTING_MANAGEMENT_VISIBLE_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: setSettingManagementVisibility,
    deps: [Injector],
    multi: true,
  },
];

export function setSettingManagementVisibility(injector: Injector) {
  return () => {
    const settingManagementHasSetting$ = injector.get(SETTING_MANAGEMENT_HAS_SETTING);
    const isSettingManagementFeatureEnable$ = injector.get(SETTING_MANAGEMENT_ROUTE_VISIBILITY);
    const routes = injector.get(RoutesService);
    combineLatest([settingManagementHasSetting$, isSettingManagementFeatureEnable$]).subscribe(
      ([settingManagementHasSetting, isSettingManagementFeatureEnable]) => {
        routes.patch(eSettingManagementRouteNames.Settings, {
          invisible: !(settingManagementHasSetting && isSettingManagementFeatureEnable),
        });
      },
    );
  };
}
