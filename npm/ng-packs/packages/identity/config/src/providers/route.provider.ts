import { eLayoutType, RoutesService } from '@abp/ng.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { APP_INITIALIZER } from '@angular/core';
import { eIdentityPolicyNames } from '../enums/policy-names';
import { eIdentityRouteNames } from '../enums/route-names';

export const IDENTITY_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: undefined,
        name: eIdentityRouteNames.IdentityManagement,
        parentName: eThemeSharedRouteNames.Administration,
        requiredPolicy: eIdentityPolicyNames.IdentityManagement,
        iconClass: 'fa fa-id-card-o',
        layout: eLayoutType.application,
        order: 1,
      },
      {
        path: '/identity/roles',
        name: eIdentityRouteNames.Roles,
        parentName: eIdentityRouteNames.IdentityManagement,
        requiredPolicy: eIdentityPolicyNames.Roles,
        order: 1,
      },
      {
        path: '/identity/users',
        name: eIdentityRouteNames.Users,
        parentName: eIdentityRouteNames.IdentityManagement,
        requiredPolicy: eIdentityPolicyNames.Users,
        order: 2,
      },
    ]);
  };
}
