import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {
  authGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';

import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { LoginComponent } from './components/login/login.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { RegisterComponent } from './components/register/register.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { eAccountComponents } from './enums/components';
import { authenticationFlowGuard } from './guards';
import { accountExtensionsResolver } from './resolvers';

const canActivate = [authenticationFlowGuard];

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: '',
    component: RouterOutletComponent,
    children: [
      {
        path: 'login',
        component: ReplaceableRouteContainerComponent,
        canActivate,
        data: {
          replaceableComponent: {
            key: eAccountComponents.Login,
            defaultComponent: LoginComponent,
          } as ReplaceableComponents.RouteData<LoginComponent>,
        },
        title: 'AbpAccount::Login',
      },
      {
        path: 'register',
        component: ReplaceableRouteContainerComponent,
        canActivate,
        data: {
          replaceableComponent: {
            key: eAccountComponents.Register,
            defaultComponent: RegisterComponent,
          } as ReplaceableComponents.RouteData<RegisterComponent>,
        },
        title: 'AbpAccount::Register',
      },
      {
        path: 'forgot-password',
        component: ReplaceableRouteContainerComponent,
        canActivate,

        data: {
          replaceableComponent: {
            key: eAccountComponents.ForgotPassword,
            defaultComponent: ForgotPasswordComponent,
          } as ReplaceableComponents.RouteData<ForgotPasswordComponent>,
        },
        title: 'AbpAccount::ForgotPassword',
      },
      {
        path: 'reset-password',
        component: ReplaceableRouteContainerComponent,
        canActivate: [],
        data: {
          tenantBoxVisible: false,
          replaceableComponent: {
            key: eAccountComponents.ResetPassword,
            defaultComponent: ResetPasswordComponent,
          } as ReplaceableComponents.RouteData<ResetPasswordComponent>,
        },
        title: 'AbpAccount::ResetPassword',
      },
      {
        path: 'manage',
        component: ReplaceableRouteContainerComponent,
        canActivate: [authGuard],
        resolve: [accountExtensionsResolver],
        data: {
          replaceableComponent: {
            key: eAccountComponents.ManageProfile,
            defaultComponent: ManageProfileComponent,
          } as ReplaceableComponents.RouteData<ManageProfileComponent>,
        },
        title: 'AbpAccount::MyAccount',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule { }
