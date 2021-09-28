import { eLayoutType, RoutesService } from '@abp/ng.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { APP_INITIALIZER } from '@angular/core';
import { debounceTime, map } from 'rxjs/operators';
import { eSettingManagementRouteNames } from '../enums/route-names';
import { SettingTabsService } from '../services/settings-tabs.service';

export const SETTING_MANAGEMENT_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
  {
    provide: APP_INITIALIZER,
    useFactory: hideRoutes,
    deps: [RoutesService, SettingTabsService],
    multi: true,
  },
];

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

export function hideRoutes(routesService: RoutesService, settingTabsService: SettingTabsService) {
  return () => {
    settingTabsService.visible$
      .pipe(
        debounceTime(0),
        map(nodes => !nodes.length),
      )
      .subscribe(invisible =>
        routesService.patch(eSettingManagementRouteNames.Settings, { invisible }),
      );
  };
}
