import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
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
      rootNamespace: 'Volo.Abp',
    },
    AbpPermissionManagement: {
      rootNamespace: 'Volo.Abp.PermissionManagement',
    },
    AbpTenantManagement: {
      rootNamespace: 'Volo.Abp.TenantManagement',
    },
    AbpIdentity: {
      rootNamespace: 'Volo.Abp',
    },
    AbpSettingManagement: {
      rootNamespace: 'Volo.Abp.SettingManagement',
    },
  },
} as Environment;
