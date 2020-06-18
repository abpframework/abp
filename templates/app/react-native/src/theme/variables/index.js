import platform from './Platform';

export const customVariables = {
  zIndex: {
    overlay: 50,
    loaders: 150,
  },
};

export const activeTheme = { ...platform, ...customVariables };
