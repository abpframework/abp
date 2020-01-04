import {
  DynamicLayoutComponent,
  AuthGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { RegisterComponent } from './components/register/register.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: '',
    component: DynamicLayoutComponent,
    children: [
      {
        path: 'login',
        component: ReplaceableRouteContainerComponent,
        data: {
          replaceableComponent: {
            key: 'Account.LoginComponent',
            defaultComponent: LoginComponent,
          } as ReplaceableComponents.RouteData<LoginComponent>,
        },
      },
      {
        path: 'register',
        component: ReplaceableRouteContainerComponent,
        data: {
          replaceableComponent: {
            key: 'Account.RegisterComponent',
            defaultComponent: RegisterComponent,
          } as ReplaceableComponents.RouteData<RegisterComponent>,
        },
      },
      {
        path: 'manage-profile',
        component: ReplaceableRouteContainerComponent,
        canActivate: [AuthGuard],
        data: {
          replaceableComponent: {
            key: 'Account.ManageProfileComponent',
            defaultComponent: ManageProfileComponent,
          } as ReplaceableComponents.RouteData<ManageProfileComponent>,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule {}
