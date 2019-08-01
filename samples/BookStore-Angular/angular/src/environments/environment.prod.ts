export const environment = {
  production: true,
  hmr: false,
  oAuthConfig: {
    issuer: 'https://localhost:44364',
    clientId: 'BookStore_ConsoleTestApp',
    dummyClientSecret: '1q2w3e*',
    scope: 'BookStore',
    showDebugInformation: true,
    oidc: false,
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44364',
    },
  },
  localization: {
    defaultResourceName: 'BookStore',
  },
};
