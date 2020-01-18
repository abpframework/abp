import {
  AuthGuard,
  DynamicLayoutComponent,
  PermissionGuard,
  CoreModule,
  ReplaceableRouteContainerComponent,
  ReplaceableComponents,
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
          replaceableComponent: {
            key: 'Identity.RolesComponent',
            defaultComponent: RolesComponent,
          } as ReplaceableComponents.RouteData<RolesComponent>,
        },
      },
      {
        path: 'users',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.Users',
          replaceableComponent: {
            key: 'Identity.UsersComponent',
            defaultComponent: UsersComponent,
          } as ReplaceableComponents.RouteData<UsersComponent>,
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
