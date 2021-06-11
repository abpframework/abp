import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'MyProjectName',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44301',
    redirectUri: baseUrl,
    clientId: 'MyProjectName_App',
    responseType: 'code',
    scope: 'offline_access MyProjectName role email openid profile',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44301',
      rootNamespace: 'MyCompanyName.MyProjectName',
    },
    MyProjectName: {
      url: 'https://localhost:44300',
      rootNamespace: 'MyCompanyName.MyProjectName',
    },
  },
} as Environment;
