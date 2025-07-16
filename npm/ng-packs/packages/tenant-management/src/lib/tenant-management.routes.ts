import { Routes } from '@angular/router';
import { TenantManagementConfigOptions } from './models/config-options';
import {
  TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';

import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { TenantsComponent } from './components';
import { eTenantManagementComponents } from './enums';
import { tenantManagementExtensionsResolver } from './resolvers';

export function provideTenantManagement(options: TenantManagementConfigOptions = {}) {
  return [
    {
      provide: TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
      useValue: options.entityActionContributors,
    },
    {
      provide: TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
      useValue: options.toolbarActionContributors,
    },
    {
      provide: TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
      useValue: options.entityPropContributors,
    },
    {
      provide: TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
      useValue: options.createFormPropContributors,
    },
    {
      provide: TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
      useValue: options.editFormPropContributors,
    },
  ];
}

export const createRoutes = (options: TenantManagementConfigOptions = {}): Routes => [
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [authGuard, permissionGuard],
    resolve: [tenantManagementExtensionsResolver],
    providers: provideTenantManagement(options),
    children: [
      { path: '', redirectTo: 'tenants', pathMatch: 'full' },
      {
        path: 'tenants',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpTenantManagement.Tenants',
          replaceableComponent: {
            key: eTenantManagementComponents.Tenants,
            defaultComponent: TenantsComponent,
          } as ReplaceableComponents.RouteData<TenantsComponent>,
        },
        title: 'AbpTenantManagement::Tenants',
      },
    ],
  },
];
