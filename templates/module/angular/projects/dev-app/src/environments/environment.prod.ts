import { Config } from '@abp/ng.core';

export const environment = {
  production: true,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'MyProjectName',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44301',
    clientId: 'MyProjectName_ConsoleTestApp',
    dummyClientSecret: '1q2w3e*',
    scope: 'MyProjectName',
    oidc: false,
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44301',
    },
    MyProjectName: {
      url: 'https://localhost:44300',
    },
  },
} as Config.Environment;
