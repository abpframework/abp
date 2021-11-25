const ENV = {
  dev: {
    apiUrl: 'http://localhost:44305',
    oAuthConfig: {
      issuer: 'http://localhost:44305',
      clientId: 'MyProjectName_App',
      clientSecret: '1q2w3e*',
      scope: 'offline_access MyProjectName',
    },
    localization: {
      defaultResourceName: 'MyProjectName',
    },
  },
  prod: {
    apiUrl: 'http://localhost:44305',
    oAuthConfig: {
      issuer: 'http://localhost:44305',
      clientId: 'MyProjectName_App',
      clientSecret: '1q2w3e*',
      scope: 'offline_access MyProjectName',
    },
    localization: {
      defaultResourceName: 'MyProjectName',
    },
  },
};

export const getEnvVars = () => {
  // eslint-disable-next-line no-undef
  return __DEV__ ? ENV.dev : ENV.prod;
};
