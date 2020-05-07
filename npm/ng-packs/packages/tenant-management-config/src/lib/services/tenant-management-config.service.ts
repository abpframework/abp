import { Injectable } from '@angular/core';
import { addAbpRoutes, eLayoutType } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class TenantManagementConfigService {
  constructor() {
    addAbpRoutes({
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
    });
  }
}
