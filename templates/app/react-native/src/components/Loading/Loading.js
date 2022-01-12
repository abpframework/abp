import React, { forwardRef } from 'react';
import { Spinner, View, connectStyle } from 'native-base';
import { StyleSheet } from 'react-native';
import PropTypes from 'prop-types';
import { activeTheme } from '../../theme/variables';
import { connectToRedux } from '../../utils/ReduxConnect';
import {
  createLoadingSelector,
  createOpacitySelector,
} from '../../store/selectors/LoadingSelectors';

function Loading({ style, loading, opacity }) {
  return loading ? (
    <View style={styles.container}>
      <View
        style={{
          ...styles.backdrop,
          opacity: opacity || 0.6,
        }}
      />
      <Spinner style={styles.spinner} color={styles.spinner.color} />
    </View>
  ) : null;
}
const Forwarded = forwardRef((props, ref) => <Loading {...props} forwardedRef={ref} />);

const backdropStyle = {
  position: 'absolute',
  top: 0,
  left: 0,
  width: '100%',
  height: '100%',
  backgroundColor: '#fff',
};

export const styles = StyleSheet.create({
  container: {
    ...backdropStyle,
    backgroundColor: 'transparent',
    zIndex: activeTheme.zIndex.indicator,
    alignItems: 'center',
    justifyContent: 'center',
  },
  backdrop: backdropStyle,
  spinner: {
    color: activeTheme.brandPrimary,
    fontSize: 100,
  },
});

Loading.propTypes = {
  style: PropTypes.objectOf(PropTypes.any),
  loading: PropTypes.bool,
  opacity: PropTypes.number,
};

export default connectToRedux({
  component: connectStyle('ABP.Loading', styles)(Forwarded),
  stateProps: state => ({
    loading: createLoadingSelector()(state),
    opacity: createOpacitySelector()(state),
  }),
});
