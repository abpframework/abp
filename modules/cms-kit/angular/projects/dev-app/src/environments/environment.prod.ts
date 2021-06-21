export const environment = {
  production: true,
  application: {
    name: 'CmsKit',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44318',
    clientId: 'CmsKit_ConsoleTestApp',
    dummyClientSecret: '1q2w3e*',
    scope: 'CmsKit',
    showDebugInformation: true,
    oidc: false,
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44318',
    },
    CmsKit: {
      url: 'https://localhost:44371',
    },
  },
};
