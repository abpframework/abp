import { Formik } from 'formik';
import i18n from 'i18n-js';
import {
  Button,
  Container,
  Content,
  Form,
  Input,
  InputGroup,
  Item,
  Label,
  Text,
  Icon,
} from 'native-base';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { View } from 'react-native';
import * as Yup from 'yup';
import { login } from '../../api/AccountAPI';
import TenantBox from '../../components/TenantBox/TenantBox';
import ValidationMessage from '../../components/ValidationMessage/ValidationMessage';
import AppActions from '../../store/actions/AppActions';
import LoadingActions from '../../store/actions/LoadingActions';
import PersistentStorageActions from '../../store/actions/PersistentStorageActions';
import { connectToRedux } from '../../utils/ReduxConnect';

const ValidationSchema = Yup.object().shape({
  username: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
  password: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
});

function LoginScreen({ startLoading, stopLoading, setToken, fetchAppConfig }) {
  const [showTenantSelection, setShowTenantSelection] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const toggleTenantSelection = () => {
    setShowTenantSelection(!showTenantSelection);
  };

  const submit = ({ username, password }) => {
    startLoading({ key: 'login' });
    login({ username, password })
      .then(data =>
        setToken({
          ...data,
          expire_time: new Date().valueOf() + data.expires_in,
          scope: undefined,
        }),
      )
      .then(
        () =>
          new Promise(resolve =>
            fetchAppConfig({ showLoading: false, callback: () => resolve(true) }),
          ),
      )
      .finally(() => stopLoading({ key: 'login' }));
  };

  return (
    <Container>
      <TenantBox
        showTenantSelection={showTenantSelection}
        toggleTenantSelection={toggleTenantSelection}
      />
      {!showTenantSelection ? (
        <Content px20 style={{ flex: 1 }}>
          <Formik
            validationSchema={ValidationSchema}
            initialValues={{ username: '', password: '' }}
            onSubmit={submit}>
            {({ handleChange, handleBlur, handleSubmit, values, errors }) => (
              <Form>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpAccount::UserNameOrEmailAddress')}</Label>
                  <Input
                    abpInput
                    onChangeText={handleChange('username')}
                    onBlur={handleBlur('username')}
                    value={values.username}
                  />
                </InputGroup>
                <ValidationMessage>{errors.username}</ValidationMessage>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpAccount::Password')}</Label>
                  <Item abpInput>
                    <Input
                      secureTextEntry={!showPassword}
                      onChangeText={handleChange('password')}
                      onBlur={handleBlur('password')}
                      value={values.password}
                    />
                    <Icon
                      name={showPassword ? 'eye-off' : 'eye'}
                      onPress={() => setShowPassword(!showPassword)}
                    />
                  </Item>
                </InputGroup>
                <ValidationMessage>{errors.password}</ValidationMessage>
                <View style={{ marginTop: 20, alignItems: 'center' }}>
                  <Button abpButton onPress={handleSubmit}>
                    <Text>{i18n.t('AbpAccount::Login')}</Text>
                  </Button>
                </View>
              </Form>
            )}
          </Formik>
        </Content>
      ) : null}
    </Container>
  );
}

LoginScreen.propTypes = {
  startLoading: PropTypes.func.isRequired,
  stopLoading: PropTypes.func.isRequired,
  setToken: PropTypes.func.isRequired,
  fetchAppConfig: PropTypes.func.isRequired,
};

export default connectToRedux({
  component: LoginScreen,
  dispatchProps: {
    startLoading: LoadingActions.start,
    stopLoading: LoadingActions.stop,
    fetchAppConfig: AppActions.fetchAppConfigAsync,
    setToken: PersistentStorageActions.setToken,
  },
});
