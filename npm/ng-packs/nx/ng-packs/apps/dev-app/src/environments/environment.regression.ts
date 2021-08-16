import { Config } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  test: true,
  production: false,
  hmr: false,
  application: {
    baseUrl,
    name: 'MyProjectName',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44305',
    redirectUri: baseUrl,
    clientId: 'MyProjectName_App',
    responseType: 'code',
    scope: 'offline_access MyProjectName',
  },
  apis: {
    default: {
      url: 'https://localhost:44305',
      rootNamespace: 'MyCompanyName.MyProjectName',
    },
    AbpFeatureManagement: {
      url: 'https://localhost:44305',
      rootNamespace: 'Volo.Abp',
    },
    AbpPermissionManagement: {
      url: 'https://localhost:44305',
      rootNamespace: 'Volo.Abp.PermissionManagement',
    },
    AbpTenantManagement: {
      url: 'https://localhost:44305',
      rootNamespace: 'Volo.Abp.TenantManagement',
    },
    AbpIdentity: {
      url: 'https://localhost:44305',
      rootNamespace: 'Volo.Abp',
    },
  },
} as Config.Environment;
