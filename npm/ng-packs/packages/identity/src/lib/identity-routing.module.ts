import { NgModule } from '@angular/core';
import { RouterModule, Routes, mapToCanActivate } from '@angular/router';

import {
  AuthGuard,
  PermissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';

import { RolesComponent } from './components/roles/roles.component';
import { UsersComponent } from './components/users/users.component';
import { eIdentityComponents } from './enums/components';
import { IdentityExtensionsGuard } from './guards';

const routes: Routes = [
  { path: '', redirectTo: 'roles', pathMatch: 'full' },
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: mapToCanActivate([AuthGuard, PermissionGuard, IdentityExtensionsGuard]),
    children: [
      {
        path: 'roles',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.Roles',
          replaceableComponent: {
            key: eIdentityComponents.Roles,
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
            key: eIdentityComponents.Users,
            defaultComponent: UsersComponent,
          } as ReplaceableComponents.RouteData<UsersComponent>,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IdentityRoutingModule {}
