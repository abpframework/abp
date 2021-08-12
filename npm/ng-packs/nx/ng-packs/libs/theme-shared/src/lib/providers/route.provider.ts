import { RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eThemeSharedRouteNames } from '../enums/route-names';

export const THEME_SHARED_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: undefined,
        name: eThemeSharedRouteNames.Administration,
        iconClass: 'fa fa-wrench',
        order: 100,
      },
    ]);
  };
}
