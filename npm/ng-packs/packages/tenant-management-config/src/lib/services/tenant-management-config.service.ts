import { Injectable } from '@angular/core';
import { addAbpRoutes, eLayoutType } from '@abp/ng.core';
import { eTenantManagementRouteNames } from '@abp/ng.tenant-management';
@Injectable({
  providedIn: 'root',
})
export class TenantManagementConfigService {
  constructor() {
    addAbpRoutes([
      {
        name: eTenantManagementRouteNames.Administration,
        path: '',
        order: 1,
        wrapper: true,
        iconClass: 'fa fa-wrench',
      },
      {
        name: eTenantManagementRouteNames.TenantManagement,
        path: 'tenant-management',
        parentName: eTenantManagementRouteNames.Administration,
        layout: eLayoutType.application,
        iconClass: 'fa fa-users',
        children: [
          {
            path: 'tenants',
            name: eTenantManagementRouteNames.Tenants,
            order: 1,
            requiredPolicy: 'AbpTenantManagement.Tenants',
          },
        ],
      },
    ]);
  }
}
