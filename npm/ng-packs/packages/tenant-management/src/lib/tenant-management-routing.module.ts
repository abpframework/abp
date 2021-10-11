import {
  AuthGuard,
  DynamicLayoutComponent,
  PermissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantsComponent } from './components/tenants/tenants.component';
import { eTenantManagementComponents } from './enums/components';
import { TenantManagementExtensionsGuard } from './guards';

const routes: Routes = [
  { path: '', redirectTo: 'tenants', pathMatch: 'full' },
  {
    path: '',
    component: DynamicLayoutComponent,
    canActivate: [AuthGuard, PermissionGuard, TenantManagementExtensionsGuard],
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
