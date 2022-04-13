import { Button, Spinner } from 'native-base';
import PropTypes from 'prop-types';
import React from 'react';
import { StyleSheet } from 'react-native';

export default function LoadingButton({ loading = false, style, children, ...props }) {
  return (
    <Button style={styles.button} {...props}>
      {children}
      {loading ? <Spinner /> : null}
    </Button>
  );
}

LoadingButton.propTypes = {
  ...Button.propTypes,
  loading: PropTypes.bool.isRequired,
};

const styles = StyleSheet.create({
  button: { marginTop: 20, marginBottom: 30, height: 30 },
});