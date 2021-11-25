// @flow

import variable from '../variables/Platform';

export default (variables /* : * */ = variable) => {
  const viewTheme = {
    '.padder': {
      padding: variables.contentPadding,
    },
  };

  return viewTheme;
};
