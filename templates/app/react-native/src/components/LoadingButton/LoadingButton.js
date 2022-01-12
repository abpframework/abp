import { Button, connectStyle, Spinner } from 'native-base';
import PropTypes from 'prop-types';
import React, { forwardRef } from 'react';
import { StyleSheet } from 'react-native';

function LoadingButton({ loading = false, style, children, ...props }) {
  return (
    <Button style={styles.button} {...props}>
      {children}
      {loading ? <Spinner style={styles.spinner} color={styles.spinner.color || 'white'} /> : null}
    </Button>
  );
}

LoadingButton.propTypes = {
  ...Button.propTypes,
  loading: PropTypes.bool.isRequired,
};

const styles = StyleSheet.create({
  button: { marginTop: 20, marginBottom: 30, height: 30 },
  spinner: {
    transform: [{ scale: 0.5 }],
    color: 'white',
    marginRight: 5,
  },
});

const Forwarded = forwardRef((props, ref) => <LoadingButton {...props} forwardedRef={ref} />);

export default connectStyle('ABP.LoadingButton', styles)(Forwarded);
