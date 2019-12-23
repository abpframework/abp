import {
  AuthGuard,
  DynamicLayoutComponent,
  PermissionGuard,
  ABP,
  CoreModule,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { NgModule, Type } from '@angular/core';
import { RouterModule, Routes, Router, ActivatedRoute } from '@angular/router';
import { RolesComponent } from './components/roles/roles.component';
import { UsersComponent } from './components/users/users.component';

const routes: Routes = [
  { path: '', redirectTo: 'roles', pathMatch: 'full' },
  {
    path: '',
    component: DynamicLayoutComponent,
    canActivate: [AuthGuard, PermissionGuard],
    children: [
      {
        path: 'roles',
        component: ReplaceableRouteContainerComponent,
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
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.Users',
          component: {
            key: 'AbpIdentity.Roles',
            default: UsersComponent,
          } as ABP.ComponentData<UsersComponent>,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes), CoreModule],
  exports: [RouterModule],
})
export class IdentityRoutingModule {}
