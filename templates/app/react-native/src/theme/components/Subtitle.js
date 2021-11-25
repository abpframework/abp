// @flow

import { Platform } from 'react-native';

import variable from '../variables/Platform';
import { PLATFORM } from '../variables/CommonColor';

export default (variables /* : * */ = variable) => {
  const subtitleTheme = {
    fontSize: variables.subTitleFontSize,
    fontFamily: variables.titleFontfamily,
    color: variables.subtitleColor,
    textAlign: 'center',
    paddingLeft: Platform.OS === PLATFORM.IOS ? 4 : 0,
    marginLeft: Platform.OS === PLATFORM.IOS ? undefined : -3,
  };

  return subtitleTheme;
};
