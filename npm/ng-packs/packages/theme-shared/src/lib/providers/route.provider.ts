import { RoutesService } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { eThemeSharedRouteNames } from '../enums/route-names';

export const THEME_SHARED_ROUTE_PROVIDERS = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

export function configureRoutes() {
  const routesService = inject(RoutesService);
  routesService.add([
    {
      path: undefined,
      name: eThemeSharedRouteNames.Administration,
      iconClass: 'fa fa-wrench',
      order: 100,
    },
  ]);
}
