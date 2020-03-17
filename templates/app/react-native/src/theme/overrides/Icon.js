// @flow
import iconTheme from '../components/Icon';

export default variables => {
  const iconOverrides = {
    ...iconTheme(variables),
    '.primary': {
      color: variables.brandPrimary,
    },
    '.success': {
      color: variables.brandSuccess,
    },
    '.info': {
      color: variables.brandInfo,
    },
    '.danger': {
      color: variables.brandDanger,
    },
    '.warning': {
      color: variables.brandWarning,
    },
    '.dark': {
      color: variables.brandDark,
    },
    '.light': {
      color: variables.brandLight,
    },
    '.navElement': {
      marginHorizontal: 15,
      fontSize: 35,
    },
  };

  return iconOverrides;
};
