import { ABP, eLayoutType } from '@abp/ng.core';

/**
 *
 * @deprecated since version 0.9.0
 */
export const TENANT_MANAGEMENT_ROUTES = {
  routes: [
    {
      name: 'AbpTenantManagement::Menu:TenantManagement',
      path: 'tenant-management',
      parentName: 'AbpUiNavigation::Menu:Administration',
      layout: eLayoutType.application,
      iconClass: 'fa fa-users',
      children: [
        {
          path: 'tenants',
          name: 'AbpTenantManagement::Tenants',
          order: 1,
          requiredPolicy: 'AbpTenantManagement.Tenants',
        },
      ],
    },
  ] as ABP.FullRoute[],
};
