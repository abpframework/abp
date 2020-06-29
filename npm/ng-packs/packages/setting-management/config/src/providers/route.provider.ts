import { eLayoutType, RoutesService, SettingTabsService } from '@abp/ng.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { APP_INITIALIZER } from '@angular/core';
import { debounceTime, map } from 'rxjs/operators';
import { eSettingManagementRouteNames } from '../enums/route-names';

export const SETTING_MANAGEMENT_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
  {
    provide: APP_INITIALIZER,
    useFactory: hideRoutes,
    deps: [RoutesService, SettingTabsService],
    multi: true,
  },
];

export function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
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

export function hideRoutes(routes: RoutesService, tabs: SettingTabsService) {
  return () => {
    tabs.visible$
      .pipe(
        debounceTime(0),
        map(nodes => !nodes.length),
      )
      .subscribe(invisible => routes.patch(eSettingManagementRouteNames.Settings, { invisible }));
  };
}
