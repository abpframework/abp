import { Provider } from '@angular/core';
import { Routes } from '@angular/router';
import {
  IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
  IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
  IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
  IDENTITY_ENTITY_PROP_CONTRIBUTORS,
  IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens';
import { IdentityConfigOptions } from './models';
import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { RolesComponent, UsersComponent } from './components';
import { identityExtensionsResolver } from './resolvers';
import { eIdentityComponents } from './enums';

export function provideIdentity(options: IdentityConfigOptions = {}): Provider[] {
  return [
    {
      provide: IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
      useValue: options.entityActionContributors,
    },
    {
      provide: IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
      useValue: options.toolbarActionContributors,
    },
    {
      provide: IDENTITY_ENTITY_PROP_CONTRIBUTORS,
      useValue: options.entityPropContributors,
    },
    {
      provide: IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
      useValue: options.createFormPropContributors,
    },
    {
      provide: IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
      useValue: options.editFormPropContributors,
    },
  ];
}

export const createRoutes = (options: IdentityConfigOptions = {}): Routes => [
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [authGuard, permissionGuard],
    resolve: [identityExtensionsResolver],
    providers: provideIdentity(options),
    children: [
      { path: '', redirectTo: 'roles', pathMatch: 'full' },
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
        title: 'AbpIdentity::Roles',
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
        title: 'AbpIdentity::Users',
      },
    ],
  },
];
