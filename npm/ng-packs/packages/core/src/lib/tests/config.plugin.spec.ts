import { RouterTestingModule } from '@angular/router/testing';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { NgxsModule, NGXS_PLUGINS, Store } from '@ngxs/store';
import { environment } from '../../../../../apps/dev-app/src/environments/environment';
import { LAYOUTS } from '@abp/ng.theme.basic';
import { RouterOutletComponent } from '../components';
import { CoreModule } from '../core.module';
import { eLayoutType } from '../enums/common';
import { ABP } from '../models';
import { ConfigPlugin, NGXS_CONFIG_PLUGIN_OPTIONS } from '../plugins';
import { ConfigState } from '../states';
import { addAbpRoutes } from '../utils';
import { OAuthModule } from 'angular-oauth2-oidc';

addAbpRoutes([
  {
    name: 'AbpUiNavigation::Menu:Administration',
    path: '',
    order: 1,
    wrapper: true,
  },
  {
    name: 'AbpIdentity::Menu:IdentityManagement',
    path: 'identity',
    order: 1,
    parentName: 'AbpUiNavigation::Menu:Administration',
    layout: eLayoutType.application,
    iconClass: 'fa fa-id-card-o',
    children: [
      { path: 'roles', name: 'AbpIdentity::Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
      { path: 'users', name: 'AbpIdentity::Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
    ],
  },
  {
    name: 'AbpAccount::Menu:Account',
    path: 'account',
    invisible: true,
    layout: eLayoutType.application,
    children: [
      { path: 'login', name: 'AbpAccount::Login', order: 1 },
      { path: 'register', name: 'AbpAccount::Register', order: 2 },
    ],
  },
  {
    name: 'AbpTenantManagement::Menu:TenantManagement',
    path: 'tenant-management',
    parentName: 'AbpUiNavigation::Menu:Administration',
    layout: eLayoutType.application,
    iconClass: 'fa fa-users',
    children: [
      {
        path: 'tenants',
        name: 'AbpTenantManagement::Tenants',
        order: 1,
        requiredPolicy: 'AbpTenantManagement.Tenants',
      },
    ],
  },
]);

const expectedState = {
  environment,
  requirements: {
    layouts: LAYOUTS,
  },
  routes: [
    {
      name: '::Menu:Home',
      path: '',
      children: [],
      url: '/',
      order: 1,
    },
    {
      name: 'AbpUiNavigation::Menu:Administration',
      path: '',
      order: 1,
      wrapper: true,
      children: [
        {
          name: 'AbpIdentity::Menu:IdentityManagement',
          path: 'identity',
          order: 1,
          parentName: 'AbpUiNavigation::Menu:Administration',
          layout: 'application',
          iconClass: 'fa fa-id-card-o',
          children: [
            {
              path: 'users',
              name: 'AbpIdentity::Users',
              order: 1,
              requiredPolicy: 'AbpIdentity.Users',
              url: '/identity/users',
            },
            {
              path: 'roles',
              name: 'AbpIdentity::Roles',
              order: 2,
              requiredPolicy: 'AbpIdentity.Roles',
              url: '/identity/roles',
            },
          ],
          url: '/identity',
        },
        {
          name: 'AbpTenantManagement::Menu:TenantManagement',
          path: 'tenant-management',
          parentName: 'AbpUiNavigation::Menu:Administration',
          layout: 'application',
          iconClass: 'fa fa-users',
          children: [
            {
              path: 'tenants',
              name: 'AbpTenantManagement::Tenants',
              order: 1,
              requiredPolicy: 'AbpTenantManagement.Tenants',
              url: '/tenant-management/tenants',
            },
          ],
          url: '/tenant-management',
          order: 2,
        },
      ],
    },
    {
      name: 'AbpAccount::Menu:Account',
      path: 'account',
      invisible: true,
      layout: 'application',
      children: [
        {
          path: 'login',
          name: 'AbpAccount::Login',
          order: 1,
          url: '/account/login',
        },
        {
          path: 'register',
          name: 'AbpAccount::Register',
          order: 2,
          url: '/account/register',
        },
      ],
      url: '/account',
      order: 2,
    },
  ],
  flattedRoutes: [
    {
      name: '::Menu:Home',
      path: '',
      children: [],
      url: '/',
      order: 1,
    },
    {
      name: 'AbpUiNavigation::Menu:Administration',
      path: '',
      order: 1,
      wrapper: true,
      children: [
        {
          name: 'AbpIdentity::Menu:IdentityManagement',
          path: 'identity',
          order: 1,
          parentName: 'AbpUiNavigation::Menu:Administration',
          layout: 'application',
          iconClass: 'fa fa-id-card-o',
          children: [
            {
              path: 'users',
              name: 'AbpIdentity::Users',
              order: 1,
              parentName: 'AbpIdentity::Menu:IdentityManagement',
              requiredPolicy: 'AbpIdentity.Users',
              url: '/identity/users',
            },
            {
              path: 'roles',
              name: 'AbpIdentity::Roles',
              order: 2,
              parentName: 'AbpIdentity::Menu:IdentityManagement',
              requiredPolicy: 'AbpIdentity.Roles',
              url: '/identity/roles',
            },
          ],
          url: '/identity',
        },
        {
          name: 'AbpTenantManagement::Menu:TenantManagement',
          path: 'tenant-management',
          parentName: 'AbpUiNavigation::Menu:Administration',
          layout: 'application',
          iconClass: 'fa fa-users',
          children: [
            {
              path: 'tenants',
              name: 'AbpTenantManagement::Tenants',
              order: 1,
              parentName: 'AbpTenantManagement::Menu:TenantManagement',
              requiredPolicy: 'AbpTenantManagement.Tenants',
              url: '/tenant-management/tenants',
            },
          ],
          url: '/tenant-management',
          order: 2,
        },
      ],
    },
    {
      name: 'AbpIdentity::Menu:IdentityManagement',
      path: 'identity',
      order: 1,
      parentName: 'AbpUiNavigation::Menu:Administration',
      layout: 'application',
      iconClass: 'fa fa-id-card-o',
      children: [
        {
          path: 'users',
          name: 'AbpIdentity::Users',
          order: 1,
          parentName: 'AbpIdentity::Menu:IdentityManagement',
          requiredPolicy: 'AbpIdentity.Users',
          url: '/identity/users',
        },
        {
          path: 'roles',
          name: 'AbpIdentity::Roles',
          order: 2,
          parentName: 'AbpIdentity::Menu:IdentityManagement',
          requiredPolicy: 'AbpIdentity.Roles',
          url: '/identity/roles',
        },
      ],
      url: '/identity',
    },
    {
      path: 'users',
      name: 'AbpIdentity::Users',
      order: 1,
      parentName: 'AbpIdentity::Menu:IdentityManagement',
      requiredPolicy: 'AbpIdentity.Users',
      url: '/identity/users',
    },
    {
      path: 'roles',
      name: 'AbpIdentity::Roles',
      order: 2,
      parentName: 'AbpIdentity::Menu:IdentityManagement',
      requiredPolicy: 'AbpIdentity.Roles',
      url: '/identity/roles',
    },
    {
      name: 'AbpTenantManagement::Menu:TenantManagement',
      path: 'tenant-management',
      parentName: 'AbpUiNavigation::Menu:Administration',
      layout: 'application',
      iconClass: 'fa fa-users',
      children: [
        {
          path: 'tenants',
          name: 'AbpTenantManagement::Tenants',
          order: 1,
          parentName: 'AbpTenantManagement::Menu:TenantManagement',
          requiredPolicy: 'AbpTenantManagement.Tenants',
          url: '/tenant-management/tenants',
        },
      ],
      url: '/tenant-management',
      order: 2,
    },
    {
      path: 'tenants',
      name: 'AbpTenantManagement::Tenants',
      order: 1,
      parentName: 'AbpTenantManagement::Menu:TenantManagement',
      requiredPolicy: 'AbpTenantManagement.Tenants',
      url: '/tenant-management/tenants',
    },
    {
      name: 'AbpAccount::Menu:Account',
      path: 'account',
      invisible: true,
      layout: 'application',
      children: [
        {
          path: 'login',
          name: 'AbpAccount::Login',
          order: 1,
          parentName: 'AbpAccount::Menu:Account',
          url: '/account/login',
        },
        {
          path: 'register',
          name: 'AbpAccount::Register',
          order: 2,
          parentName: 'AbpAccount::Menu:Account',
          url: '/account/register',
        },
      ],
      url: '/account',
      order: 2,
    },
    {
      path: 'login',
      name: 'AbpAccount::Login',
      order: 1,
      parentName: 'AbpAccount::Menu:Account',
      url: '/account/login',
    },
    {
      path: 'register',
      name: 'AbpAccount::Register',
      order: 2,
      parentName: 'AbpAccount::Menu:Account',
      url: '/account/register',
    },
  ],
};

describe('ConfigPlugin', () => {
  let spectator: SpectatorService<ConfigPlugin>;
  const createService = createServiceFactory({
    service: ConfigPlugin,
    imports: [
      CoreModule,
      OAuthModule.forRoot(),
      NgxsModule.forRoot([]),
      RouterTestingModule.withRoutes([
        {
          path: '',
          component: RouterOutletComponent,
          data: {
            routes: {
              name: '::Menu:Home',
            } as ABP.Route,
          },
        },
        { path: 'identity', component: RouterOutletComponent },
        { path: 'account', component: RouterOutletComponent },
        { path: 'tenant-management', component: RouterOutletComponent },
      ]),
    ],
    providers: [
      {
        provide: NGXS_PLUGINS,
        useClass: ConfigPlugin,
        multi: true,
      },
      {
        provide: NGXS_CONFIG_PLUGIN_OPTIONS,
        useValue: { environment, requirements: { layouts: LAYOUTS } } as ABP.Root,
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
  });

  it('should ConfigState must be create with correct datas', () => {
    const store = spectator.get(Store);
    const state = store.selectSnapshot(ConfigState);
    expect(state).toEqual(expectedState);
  });
});
