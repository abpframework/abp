import { AuthGuard, DynamicLayoutComponent, PermissionGuard, ABP, CoreModule } from '@abp/ng.core';
import { NgModule, Type } from '@angular/core';
import { RouterModule, Routes, Router, ActivatedRoute } from '@angular/router';
import { RolesComponent } from './components/roles/roles.component';
import { UsersComponent } from './components/users/users.component';
import { RouteWrapperComponent } from './components/route-wrapper.component';

const routes: Routes = [
  { path: '', redirectTo: 'roles', pathMatch: 'full' },
  {
    path: '',
    component: DynamicLayoutComponent,
    canActivate: [AuthGuard, PermissionGuard],
    children: [
      {
        path: 'roles',
        component: RouteWrapperComponent,
        data: {
          requiredPolicy: 'AbpIdentity.Roles',
          component: {
            key: 'AbpIdentity.Roles',
            default: RolesComponent,
          } as ABP.ComponentData<RolesComponent>,
        },
      },
      {
        path: 'users',
        component: UsersComponent,
        data: { requiredPolicy: 'AbpIdentity.Users' },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes), CoreModule],
  exports: [RouterModule],
})
export class IdentityRoutingModule {}
