import { eLayoutType, noop, RoutesService, SettingTabsService } from '@abp/ng.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { APP_INITIALIZER, inject, InjectionToken } from '@angular/core';
import { debounceTime, map } from 'rxjs/operators';
import { eSettingManagementRouteNames } from '../enums/route-names';
import { Observable } from 'rxjs';

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        name: eSettingManagementRouteNames.Settings,
        path: '/setting-management',
        parentName: eThemeSharedRouteNames.Administration,
        layout: eLayoutType.application,
        order: 100,
        iconClass: 'fa fa-cog',
      },
    ]);
  };
}
export const SETTING_MANAGEMENT_HAS_SETTING = new InjectionToken<Observable<boolean>>(
  'SETTING_MANAGEMENT_HAS_SETTING',
  {
    factory: () => {
      const settingTabsService = inject(SettingTabsService);
      return settingTabsService.visible$.pipe(
        debounceTime(0),
        map(nodes => !!nodes.length),
      );
    },
  },
);

export const SETTING_MANAGEMENT_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
  {
    provide: APP_INITIALIZER,
    useFactory: noop,
    deps: [SETTING_MANAGEMENT_HAS_SETTING],
    multi: true,
  },
];
