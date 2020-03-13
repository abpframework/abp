// @flow

import variable from '../variables/Platform';

export default (variables /* : * */ = variable) => {
  const iconTheme = {
    fontSize: variables.iconFontSize,
    color: variable.textColor,
  };

  return iconTheme;
};
