import { useFormik } from 'formik';
import i18n from 'i18n-js';
import {
  Box,
  Button,
  Center,
  FormControl, Image, Input,
  Stack,
  WarningOutlineIcon
} from 'native-base';
import PropTypes from 'prop-types';
import React, { useRef, useState } from 'react';
import { View } from 'react-native';
import { object, string } from 'yup';
import { login } from '../../api/AccountAPI';
import TenantBox from '../../components/TenantBox/TenantBox';
import ValidationMessage from '../../components/ValidationMessage/ValidationMessage';
import AppActions from '../../store/actions/AppActions';
import LoadingActions from '../../store/actions/LoadingActions';
import PersistentStorageActions from '../../store/actions/PersistentStorageActions';
import { connectToRedux } from '../../utils/ReduxConnect';

const ValidationSchema = object().shape({
  username: string().required('AbpAccount::ThisFieldIsRequired.'),
  password: string().required('AbpAccount::ThisFieldIsRequired.'),
});

function LoginScreen({ startLoading, stopLoading, setToken, fetchAppConfig }) {
  const [showTenantSelection, setShowTenantSelection] = useState(false);
  const passwordRef = useRef(null);

  const toggleTenantSelection = () => {
    setShowTenantSelection(!showTenantSelection);
  };

  const submit = ({ username, password }) => {
    startLoading({ key: 'login' });
    login({ username, password })
      .then((data) =>
        setToken({
          ...data,
          expire_time: new Date().valueOf() + data.expires_in,
          scope: undefined,
        })
      )
      .then(
        () =>
          new Promise((resolve) =>
            fetchAppConfig({
              showLoading: false,
              callback: () => resolve(true),
            })
          )
      )
      .finally(() => stopLoading({ key: 'login' }));
  };

  const formik = useFormik({
    validationSchema: ValidationSchema,
    initialValues: { username: '', password: '' },
    onSubmit: submit,
  });

  return (
    <Center flex={0.6} px="3">
      <Box
        w={{
          base: '100%',
        }}
        mb="50"
        alignItems="center"
      >
        <Image
          alt="Image"
          source={require('../../../assets/logo.png')}
        />
      </Box>

      <TenantBox
        showTenantSelection={showTenantSelection}
        toggleTenantSelection={toggleTenantSelection}
      />
      <Box
        w={{
          base: '100%',
        }}
        display={showTenantSelection ? 'none' : 'flex'}
      >
        <FormControl isRequired my="2">
          <Stack mx="4">
            <FormControl.Label>
              {i18n.t('AbpAccount::UserNameOrEmailAddress')}
            </FormControl.Label>
            <Input
              onChangeText={formik.handleChange('username')}
              onBlur={formik.handleBlur('username')}
              value={formik.values.username}
              returnKeyType="next"
              autoCapitalize="none"
              onSubmitEditing={() => passwordRef?.current?.focus()}
              size="lg"
            />
            <ValidationMessage>{formik.errors.username}</ValidationMessage>
          </Stack>
        </FormControl>

        <FormControl isRequired my="2">
          <Stack mx="4">
            <FormControl.Label>
              {i18n.t('AbpAccount::Password')}
            </FormControl.Label>
            <Input
              type="password"
              onChangeText={formik.handleChange('password')}
              onBlur={formik.handleBlur('password')}
              value={formik.values.password}
              ref={passwordRef}
              autoCapitalize="none"
              size="lg"
            />
            <FormControl.ErrorMessage
              leftIcon={<WarningOutlineIcon size="xs" />}
            >
              {formik.errors.password}
            </FormControl.ErrorMessage>
          </Stack>
        </FormControl>

        <View style={{ marginTop: 20, alignItems: 'center' }}>
          <Button onPress={formik.handleSubmit} width="30%" size="lg">
            {i18n.t('AbpAccount::Login')}
          </Button>
        </View>
      </Box>
    </Center>
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
