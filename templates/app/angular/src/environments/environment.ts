export const environment = {
  production: false,
  hmr: false,
  oAuthConfig: {
    issuer: 'https://localhost:44305',
    clientId: 'MyProjectName_ConsoleTestApp',
    dummyClientSecret: '1q2w3e*',
    scope: 'MyProjectName',
    showDebugInformation: true,
    oidc: false,
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44305',
    },
  },
  localization: {
    defaultResourceName: 'MyProjectName',
  },
};
