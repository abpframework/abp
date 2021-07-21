import {
  AuthGuard,
  DynamicLayoutComponent,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { LoginComponent } from './components/login/login.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { RegisterComponent } from './components/register/register.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { eAccountComponents } from './enums/components';
import { AuthenticationFlowGuard } from './guards/authentication-flow.guard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: '',
    component: DynamicLayoutComponent,
    children: [
      {
        path: 'login',
        component: ReplaceableRouteContainerComponent,
        canActivate: [AuthenticationFlowGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.Login,
            defaultComponent: LoginComponent,
          } as ReplaceableComponents.RouteData<LoginComponent>,
        },
      },
      {
        path: 'register',
        component: ReplaceableRouteContainerComponent,
        canActivate: [AuthenticationFlowGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.Register,
            defaultComponent: RegisterComponent,
          } as ReplaceableComponents.RouteData<RegisterComponent>,
        },
      },
      {
        path: 'forgot-password',
        component: ReplaceableRouteContainerComponent,
        canActivate: [AuthenticationFlowGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.ForgotPassword,
            defaultComponent: ForgotPasswordComponent,
          } as ReplaceableComponents.RouteData<ForgotPasswordComponent>,
        },
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
      },
      {
        path: 'manage',
        component: ReplaceableRouteContainerComponent,
        canActivate: [AuthGuard],
        data: {
          replaceableComponent: {
            key: eAccountComponents.ManageProfile,
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
