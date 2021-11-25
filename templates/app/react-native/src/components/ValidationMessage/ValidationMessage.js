import i18n from 'i18n-js';
import { connectStyle } from 'native-base';
import React, { forwardRef } from 'react';
import { Text } from 'react-native';

const ValidationMessage = ({ children, ...props }) =>
  children ? <Text {...props}>{i18n.t(children)}</Text> : null;

const styles = {
  fontSize: 12,
  marginHorizontal: 10,
  marginTop: -5,
  color: '#ed2f2f',
};

const Forwarded = forwardRef((props, ref) => <ValidationMessage {...props} forwardedRef={ref} />);

export default connectStyle('ABP.ValidationMessage', styles)(Forwarded);
