export const environment = {
  production: false,
  hmr: true,
  application: {
    name: 'MyProjectName',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44301',
    clientId: 'MyProjectName_ConsoleTestApp',
    dummyClientSecret: '1q2w3e*',
    scope: 'MyProjectName',
    showDebugInformation: true,
    oidc: false,
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44300',
    },
  },
  localization: {
    defaultResourceName: 'MyProjectName',
  },
};
