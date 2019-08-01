import { ABP, eLayoutType } from '@abp/ng.core';

export const TENANT_MANAGEMENT_ROUTES = [
  {
    name: 'Tenant Management',
    path: 'tenant-management',
    parentName: 'Administration',
    layout: eLayoutType.application,
    children: [
      {
        path: 'tenants',
        name: 'Tenants',
        order: 1,
        requiredPolicy: 'AbpTenantManagement.Tenants',
      },
    ],
  },
] as ABP.FullRoute[];
