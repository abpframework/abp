import { useFormik } from 'formik';
import i18n from 'i18n-js';
import {
  Box,
  FormControl,
  Input,
  KeyboardAvoidingView,
  Stack,
  Icon
} from 'native-base';
import PropTypes from 'prop-types';
import React, { useRef, useState } from 'react';
import * as Yup from 'yup';
import { FormButtons } from '../../components/FormButtons';
import ValidationMessage from '../../components/ValidationMessage/ValidationMessage';
import { Ionicons } from '@expo/vector-icons';

const ValidationSchema = Yup.object().shape({
  currentPassword: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
  newPassword: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
});

function ChangePasswordForm({ submit, cancel }) {
  const [showCurrentPassword, setShowCurrentPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);

  const currentPasswordRef = useRef();
  const newPasswordRef = useRef();

  const onSubmit = (values) => {
    submit({
      ...values,
      newPasswordConfirm: values.newPassword,
    });
  };

  const formik = useFormik({
    enableReinitialize: true,
    validationSchema: ValidationSchema,
    initialValues: {
      currentPassword: '',
      newPassword: '',
    },
    onSubmit,
  });

  return (
    <>
      <Box px="3">
        <FormControl isRequired my="2">
          <Stack mx="4">
            <FormControl.Label>
              {i18n.t('AbpIdentity::DisplayName:CurrentPassword')}
            </FormControl.Label>
            <Input
              ref={currentPasswordRef}
              onSubmitEditing={() => newPasswordRef?.current?.focus()}
              returnKeyType="next"
              onChangeText={formik.handleChange('currentPassword')}
              onBlur={formik.handleBlur('currentPassword')}
              value={formik.values.currentPassword}
              textContentType="password"
              secureTextEntry={!showCurrentPassword}
              InputRightElement={
                <Icon
                  as={Ionicons}
                  size="5"
                  mr="2"
                  name={showCurrentPassword ? 'eye-off-outline' : 'eye-outline'}
                  onPress={() => setShowCurrentPassword(!showCurrentPassword)}
                />
              }
            />
            <ValidationMessage>
              {formik.errors.currentPassword}
            </ValidationMessage>
          </Stack>
        </FormControl>

        <FormControl isRequired my="2">
          <Stack mx="4">
            <FormControl.Label>
              {i18n.t('AbpIdentity::DisplayName:NewPassword')}
            </FormControl.Label>
            <Input
              ref={newPasswordRef}
              returnKeyType="done"
              onChangeText={formik.handleChange('newPassword')}
              onBlur={formik.handleBlur('newPassword')}
              value={formik.values.newPassword}
              textContentType="newPassword"
              secureTextEntry={!showNewPassword}
              InputRightElement={
                <Icon
                  as={Ionicons}
                  size="5"
                  mr="2"
                  name={showNewPassword ? 'eye-off-outline' : 'eye-outline'}
                  onPress={() => setShowNewPassword(!showNewPassword)}
                />
              }
            />
            <ValidationMessage>{formik.errors.newPassword}</ValidationMessage>
          </Stack>
        </FormControl>
      </Box>
      <FormButtons
        submit={formik.handleSubmit}
        cancel={cancel}
        isSubmitDisabled={!formik.isValid}
      />
    </>
  );
}

ChangePasswordForm.propTypes = {
  submit: PropTypes.func.isRequired,
  cancel: PropTypes.func.isRequired,
};

export default ChangePasswordForm;
