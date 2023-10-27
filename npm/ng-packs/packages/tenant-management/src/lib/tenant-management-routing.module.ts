import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {
  AuthGuardFn,
  PermissionGuardFn,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';

import { TenantsComponent } from './components/tenants/tenants.component';
import { eTenantManagementComponents } from './enums/components';
import { TenantManagementExtensionsGuardFn } from './guards';

const routes: Routes = [
  { path: '', redirectTo: 'tenants', pathMatch: 'full' },
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [AuthGuardFn, PermissionGuardFn, TenantManagementExtensionsGuardFn],
    children: [
      {
        path: 'tenants',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpTenantManagement.Tenants',
          replaceableComponent: {
            key: eTenantManagementComponents.Tenants,
            defaultComponent: TenantsComponent,
          } as ReplaceableComponents.RouteData<TenantsComponent>,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TenantManagementRoutingModule {}
