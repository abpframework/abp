import { ABP, eLayoutType } from '@abp/ng.core';

export const TENANT_MANAGEMENT_ROUTES = [
  {
    name: 'TenantManagement',
    path: 'tenant-management',
    parentName: 'Administration',
    layout: eLayoutType.application,
    children: [
      {
        path: 'tenants',
        name: 'Tenants',
        order: 1,
        requiredPolicy: 'AbpTenantManagement.Tenants',
        parentName: 'TenantManagement',
      },
    ],
  },
] as ABP.FullRoute[];
