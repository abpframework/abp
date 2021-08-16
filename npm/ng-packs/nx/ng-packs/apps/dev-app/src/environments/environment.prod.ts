import { Config } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
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
    },
  },
} as Config.Environment;
