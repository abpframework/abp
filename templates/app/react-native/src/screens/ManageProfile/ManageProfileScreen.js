import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { updateProfileDetail, getProfileDetail } from '../../api/IdentityAPI';
import ManageProfileForm from './ManageProfileForm';
import LoadingActions from '../../store/actions/LoadingActions';
import { connectToRedux } from '../../utils/ReduxConnect';

function ManageProfileScreen({ navigation, startLoading, stopLoading }) {
  const [user, setUser] = useState();

  useEffect(() => {
    if (!user) {
      startLoading({ key: 'manageProfile' });
      getProfileDetail()
        .then((data = {}) => setUser(data))
        .finally(() => stopLoading({ key: 'manageProfile' }));
    }
  });

  const submit = data => {
    startLoading({ key: 'manageProfile' });

    updateProfileDetail(data)
      .then(() => {
        navigation.goBack();
      })
      .finally(() => stopLoading({ key: 'manageProfile' }));
  };

  return (
    <>
      {user ? (
        <ManageProfileForm editingUser={user} submit={submit} cancel={() => navigation.goBack()} />
      ) : null}
    </>
  );
}

ManageProfileScreen.propTypes = {
  startLoading: PropTypes.func.isRequired,
  stopLoading: PropTypes.func.isRequired,
};

export default connectToRedux({
  component: ManageProfileScreen,
  dispatchProps: {
    startLoading: LoadingActions.start,
    stopLoading: LoadingActions.stop,
  },
});
