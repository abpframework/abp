import { Injectable } from '@angular/core';
import { ABP_ROUTES, eLayoutType } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class TenantManagementConfigService {
  constructor() {
    ABP_ROUTES.push({
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
