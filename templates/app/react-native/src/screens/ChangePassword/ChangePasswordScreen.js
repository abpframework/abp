import React from 'react';
import PropTypes from 'prop-types';
import { changePassword } from '../../api/IdentityAPI';
import LoadingActions from '../../store/actions/LoadingActions';
import { connectToRedux } from '../../utils/ReduxConnect';
import ChangePasswordForm from './ChangePasswordForm';

function ChangePasswordScreen({ navigation, startLoading, stopLoading }) {
  const submit = data => {
    startLoading({ key: 'changePassword' });

    changePassword(data)
      .then(() => {
        navigation.goBack();
      })
      .finally(() => stopLoading({ key: 'changePassword' }));
  };

  return <ChangePasswordForm submit={submit} cancel={() => navigation.goBack()} />;
}

ChangePasswordScreen.propTypes = {
  startLoading: PropTypes.func.isRequired,
  stopLoading: PropTypes.func.isRequired,
};

export default connectToRedux({
  component: ChangePasswordScreen,
  dispatchProps: {
    startLoading: LoadingActions.start,
    stopLoading: LoadingActions.stop,
  },
});
