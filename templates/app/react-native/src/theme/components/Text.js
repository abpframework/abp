// @flow

import variable from '../variables/Platform';

export default (variables /* : * */ = variable) => {
  const textTheme = {
    fontSize: variables.DefaultFontSize,
    fontFamily: variables.fontFamily,
    color: variables.textColor,
    '.note': {
      color: '#a7a7a7',
      fontSize: variables.noteFontSize,
    },
    '.textPrimary': {
      color: '#333',
    },
  };

  return textTheme;
};
