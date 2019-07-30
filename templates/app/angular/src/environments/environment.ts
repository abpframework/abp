export const environment = {
  production: false,
  hmr: false,
  oAuthConfig: {
    issuer: 'http://localhost:44392',
    clientId: 'MyProjectName_ConsoleTestApp',
    dummyClientSecret: '1q2w3e*',
    scope: 'MyProjectName',
    showDebugInformation: true,
    oidc: false,
    requireHttps: false,
  },
  apis: {
    default: {
      url: 'http://localhost:44392',
    },
  },
  localization: {
    defaultResourceName: 'MyProjectName',
  },
};
