import i18n from 'i18n-js';
import React, { forwardRef } from 'react';
import { Text } from 'react-native';

const ValidationMessage = ({ children, ...props }) =>
  children ? <Text style={styles} {...props}>{i18n.t(children)}</Text> : null;

const styles = {
  fontSize: 12,
  marginTop: 3,
  color: '#ed2f2f',
};

const Forwarded = forwardRef((props, ref) => <ValidationMessage {...props} forwardedRef={ref} />);

export default Forwarded
