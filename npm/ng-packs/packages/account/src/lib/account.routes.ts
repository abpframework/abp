import { Routes } from '@angular/router';
import {
  ForgotPasswordComponent,
  LoginComponent,
  RegisterComponent,
  ResetPasswordComponent,
  ManageProfileComponent,
} from './components';
import { eAccountComponents } from './enums';
import { authenticationFlowGuard } from './guards';
import { accountExtensionsResolver } from './resolvers';
import {
  RE_LOGIN_CONFIRMATION_TOKEN,
  ACCOUNT_CONFIG_OPTIONS,
  ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS,
} from './tokens';
import { AccountConfigOptions } from './models';
import { accountConfigOptionsFactory } from './utils';
import {
  authGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';

export function provideAccount(options: AccountConfigOptions = {}) {
  return [
    { provide: ACCOUNT_CONFIG_OPTIONS, useValue: options },
    {
      provide: 'ACCOUNT_OPTIONS',
      useFactory: accountConfigOptionsFactory,
      deps: [ACCOUNT_CONFIG_OPTIONS],
    },
    {
      provide: RE_LOGIN_CONFIRMATION_TOKEN,
      useValue: options.isPersonalSettingsChangedConfirmationActive ?? true,
    },
    {
      provide: ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS,
      useValue: options.editFormPropContributors,
    },
  ];
}

const canActivate = [authenticationFlowGuard];

export const createRoutes = (options: AccountConfigOptions = {}): Routes => [
  {
    path: '',
    component: RouterOutletComponent,
    providers: provideAccount(options),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'login' },
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
