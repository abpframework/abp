import { AuthGuard, DynamicLayoutComponent, PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantsResolver } from './resolvers/tenants.resolver';
import { TenantsComponent } from './components/tenants/tenants.component';

const routes: Routes = [
  { path: '', redirectTo: 'tenants', pathMatch: 'full' },
  {
    path: 'tenants',
    component: DynamicLayoutComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { requiredPolicy: 'AbpTenantManagement.Tenants' },
    children: [{ path: '', component: TenantsComponent, resolve: [TenantsResolver] }],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [TenantsResolver],
})
export class TenantManagementRoutingModule {}
