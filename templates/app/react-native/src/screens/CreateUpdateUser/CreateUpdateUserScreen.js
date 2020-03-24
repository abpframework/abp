import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { createUser, getUserById, removeUser, updateUser } from '../../api/IdentityAPI';
import LoadingActions from '../../store/actions/LoadingActions';
import { createLoadingSelector } from '../../store/selectors/LoadingSelectors';
import { connectToRedux } from '../../utils/ReduxConnect';
import CreateUpdateUserForm from './CreateUpdateUserForm';

function CreateUpdateUserScreen({ navigation, route, startLoading, stopLoading }) {
  const [user, setUser] = useState();
  const userId = route.params?.userId;

  const remove = () => {
    startLoading({ key: 'remove user' });
    removeUser(userId)
      .then(() => navigation.goBack())
      .finally(() => stopLoading({ key: 'remove user' }));
  };

  useEffect(() => {
    if (userId) {
      getUserById(userId).then((data = {}) => setUser(data));
    }
  }, []);

  const submit = data => {
    startLoading({ key: 'saveUser' });
    let request;
    if (data.id) {
      request = updateUser(data, userId);
    } else {
      request = createUser(data);
    }

    request
      .then(() => {
        navigation.goBack();
      })
      .finally(() => stopLoading({ key: 'saveUser' }));
  };

  const renderForm = () => (
    <CreateUpdateUserForm editingUser={user} submit={submit} remove={remove} />
  );

  if (userId && user) {
    return renderForm();
  }

  if (!userId) {
    return renderForm();
  }

  return null;
}

CreateUpdateUserScreen.propTypes = {
  startLoading: PropTypes.func.isRequired,
  stopLoading: PropTypes.func.isRequired,
};

export default connectToRedux({
  component: CreateUpdateUserScreen,
  stateProps: state => ({ loading: createLoadingSelector()(state) }),
  dispatchProps: {
    startLoading: LoadingActions.start,
    stopLoading: LoadingActions.stop,
  },
});
