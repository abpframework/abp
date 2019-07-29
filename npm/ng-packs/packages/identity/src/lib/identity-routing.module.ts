import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RolesComponent } from './components/roles/roles.component';
import { RoleResolver } from './resolvers/roles.resolver';
import { DynamicLayoutComponent, AuthGuard, PermissionGuard } from '@abp/ng.core';
import { UsersComponent } from './components/users/users.component';
import { UserResolver } from './resolvers/users.resolver';

const routes: Routes = [
  { path: '', redirectTo: 'roles', pathMatch: 'full' },
  {
    path: 'roles',
    component: DynamicLayoutComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { requiredPolicy: 'AbpIdentity.Roles' },
    children: [{ path: '', component: RolesComponent, resolve: [RoleResolver] }],
  },
  {
    path: 'users',
    component: DynamicLayoutComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { requiredPolicy: 'AbpIdentity.Users' },
    children: [
      {
        path: '',
        component: UsersComponent,
        resolve: [RoleResolver, UserResolver],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [RoleResolver, UserResolver],
})
export class IdentityRoutingModule {}
