import { eLayoutType, RoutesService } from '@abp/ng.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { inject, provideAppInitializer } from '@angular/core';
import { eTenantManagementPolicyNames } from '../enums/policy-names';
import { eTenantManagementRouteNames } from '../enums/route-names';

export const TENANT_MANAGEMENT_ROUTE_PROVIDERS = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

export function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
    {
      path: undefined,
      name: eTenantManagementRouteNames.TenantManagement,
      parentName: eThemeSharedRouteNames.Administration,
      requiredPolicy: eTenantManagementPolicyNames.TenantManagement,
      layout: eLayoutType.application,
      iconClass: 'fa fa-users',
      order: 2,
    },
    {
      path: '/tenant-management/tenants',
      name: eTenantManagementRouteNames.Tenants,
      parentName: eTenantManagementRouteNames.TenantManagement,
      requiredPolicy: eTenantManagementPolicyNames.Tenants,
      order: 1,
    },
  ]);
}
