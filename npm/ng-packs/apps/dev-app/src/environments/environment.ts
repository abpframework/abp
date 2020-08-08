import { Config } from '@abp/ng.core';

export const environment = {
  production: false,
  hmr: false,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'MyProjectName',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44305',
    clientId: 'MyProjectName_App',
    dummyClientSecret: '1q2w3e*',
    scope: 'MyProjectName',
    oidc: false,
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44305',
    },
  },
} as Config.Environment;
