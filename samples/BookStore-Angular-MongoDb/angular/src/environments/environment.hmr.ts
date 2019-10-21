export const environment = {
  production: false,
  hmr: true,
  application: {
    name: 'BookStore',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44341',
    clientId: 'BookStore_App',
    dummyClientSecret: '1q2w3e*',
    scope: 'BookStore',
    showDebugInformation: true,
    oidc: false,
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44341',
    },
  },
  localization: {
    defaultResourceName: 'BookStore',
  },
};
