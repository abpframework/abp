import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eAccountRouteNames } from '../enums/route-names';

export const ACCOUNT_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/account',
        name: eAccountRouteNames.Account,
        invisible: true,
        layout: eLayoutType.application,
        order: 1,
      },
      {
        path: '/account/login',
        name: eAccountRouteNames.Login,
        parentName: eAccountRouteNames.Account,
        order: 1,
      },
      {
        path: '/account/register',
        name: eAccountRouteNames.Register,
        parentName: eAccountRouteNames.Account,
        order: 2,
      },
      {
        path: '/account/manage-profile',
        name: eAccountRouteNames.ManageProfile,
        parentName: eAccountRouteNames.Account,
        order: 3,
      },
    ]);
  };
}
