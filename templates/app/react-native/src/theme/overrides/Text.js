// @flow
import textTheme from '../components/Text';

export default variables => {
  const textOverrides = {
    ...textTheme(variables),
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
  };

  return textOverrides;
};
