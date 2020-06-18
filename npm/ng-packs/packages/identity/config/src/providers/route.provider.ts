import { eLayoutType, RoutesService } from '@abp/ng.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { APP_INITIALIZER } from '@angular/core';
import { eIdentityRouteNames } from '../enums/route-names';

export const IDENTITY_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/identity',
        name: eIdentityRouteNames.IdentityManagement,
        parentName: eThemeSharedRouteNames.Administration,
        iconClass: 'fa fa-id-card-o',
        layout: eLayoutType.application,
        order: 1,
      },
      {
        path: '/identity/roles',
        name: eIdentityRouteNames.Roles,
        parentName: eIdentityRouteNames.IdentityManagement,
        requiredPolicy: 'AbpIdentity.Roles',
        order: 1,
      },
      {
        path: '/identity/users',
        name: eIdentityRouteNames.Users,
        parentName: eIdentityRouteNames.IdentityManagement,
        requiredPolicy: 'AbpIdentity.Users',
        order: 2,
      },
    ]);
  };
}
