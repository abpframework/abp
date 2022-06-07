import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Angular',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44301/',
    redirectUri: baseUrl,
    clientId: 'AbpApp',
    dummyClientSecret: '1q2w3e*',
    responseType: 'code',
    scope: 'offline_access AbpAPI',
    requireHttps: false,
  },
  apis: {
    default: {
      url: 'https://localhost:44301',
      rootNamespace: 'OpenIddict.Demo.Client.Angular',
    },
  },
} as Environment;
