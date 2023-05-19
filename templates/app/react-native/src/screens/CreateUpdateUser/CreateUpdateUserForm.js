import { Ionicons } from '@expo/vector-icons';
import { useFormik } from 'formik';
import i18n from 'i18n-js';
import {
  Box,
  Button,
  Checkbox,
  FormControl,
  Icon,
  Input,
  KeyboardAvoidingView,
  Stack
} from 'native-base';
import PropTypes from 'prop-types';
import React, { useRef, useState } from 'react';
import { Platform } from 'react-native';
import * as Yup from 'yup';
import { FormButtons } from '../../components/FormButtons';
import ValidationMessage from '../../components/ValidationMessage/ValidationMessage';
import { usePermission } from '../../hooks/UsePermission';
import UserRoles from './UserRoles';

const validations = {
  userName: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
  email: Yup.string()
    .email('AbpAccount::ThisFieldIsNotAValidEmailAddress.')
    .required('AbpAccount::ThisFieldIsRequired.'),
};

let roleNames = [];

function onChangeRoles(roles) {
  roleNames = roles;
}

function CreateUpdateUserForm({ editingUser = {}, submit, remove }) {
  const [selectedTab, setSelectedTab] = useState(0);
  const [showPassword, setShowPassword] = useState(false);

  const usernameRef = useRef();
  const nameRef = useRef();
  const surnameRef = useRef();
  const emailRef = useRef();
  const phoneNumberRef = useRef();
  const passwordRef = useRef();

  const hasRemovePermission = usePermission('AbpIdentity.Users.Delete');

  const onSubmit = (values) => {
    submit({
      ...editingUser,
      ...values,
      roleNames,
    });
  };

  const passwordValidation = Yup.lazy(() => {
    if (editingUser.id) {
      return Yup.string();
    }
    return Yup.string().required('AbpAccount::ThisFieldIsRequired.');
  });

  const formik = useFormik({
    enableReinitialize: true,
    validationSchema: Yup.object().shape({
      ...validations,
      password: passwordValidation,
    }),
    initialValues: {
      lockoutEnabled: false,
      ...editingUser,
    },
    onSubmit,
  });

  return (
    <>
      <KeyboardAvoidingView
        h={{
          base: '400px',
          lg: 'auto',
        }}
        behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      >
        <Box w={{ base: '100%' }} px="4">
          <Button.Group
            colorScheme="blue"
            mx={{
              base: 'auto',
              md: 0,
            }}
            size="sm"
            m="2"
          >
            <Button
              size="sm"
              variant={selectedTab === 0 ? 'solid' : 'outline'}
              onPress={() => setSelectedTab(0)}
            >
              {i18n.t('AbpIdentity::UserInformations')}
            </Button>
            <Button
              size="sm"
              variant={selectedTab === 1 ? 'solid' : 'outline'}
              onPress={() => setSelectedTab(1)}
            >
              {i18n.t('AbpIdentity::Roles')}
            </Button>
          </Button.Group>

          {selectedTab === 0 ? (
            <>
              <FormControl isRequired my="2">
                <Stack mx="4">
                  <FormControl.Label>
                    {i18n.t('AbpIdentity::UserName')}
                  </FormControl.Label>
                  <Input
                    ref={usernameRef}
                    onSubmitEditing={() => nameRef.current.focus()}
                    returnKeyType="next"
                    onChangeText={formik.handleChange('userName')}
                    onBlur={formik.handleBlur('userName')}
                    value={formik.values.userName}
                    autoCapitalize="none"
                  />
                  <ValidationMessage>
                    {formik.errors.userName}
                  </ValidationMessage>
                </Stack>
              </FormControl>

              <FormControl my="2">
                <Stack mx="4">
                  <FormControl.Label>
                    {i18n.t('AbpIdentity::DisplayName:Name')}
                  </FormControl.Label>
                  <Input
                    ref={nameRef}
                    onSubmitEditing={() => surnameRef.current.focus()}
                    returnKeyType="next"
                    onChangeText={formik.handleChange('name')}
                    onBlur={formik.handleBlur('name')}
                    value={formik.values.name}
                  />
                </Stack>
              </FormControl>

              <FormControl my="2">
                <Stack mx="4">
                  <FormControl.Label>
                    {i18n.t('AbpIdentity::DisplayName:Surname')}
                  </FormControl.Label>
                  <Input
                    ref={surnameRef}
                    onSubmitEditing={() => emailRef.current.focus()}
                    returnKeyType="next"
                    onChangeText={formik.handleChange('surname')}
                    onBlur={formik.handleBlur('surname')}
                    value={formik.values.surname}
                  />
                </Stack>
              </FormControl>

              <FormControl isRequired my="2">
                <Stack mx="4">
                  <FormControl.Label>
                    {i18n.t('AbpIdentity::EmailAddress')}
                  </FormControl.Label>
                  <Input
                    ref={emailRef}
                    onSubmitEditing={() => phoneNumberRef.current.focus()}
                    returnKeyType="next"
                    onChangeText={formik.handleChange('email')}
                    onBlur={formik.handleBlur('email')}
                    value={formik.values.email}
                    autoCapitalize="none"
                  />
                  <ValidationMessage>{formik.errors.email}</ValidationMessage>
                </Stack>
              </FormControl>

              <FormControl my="2">
                <Stack mx="4">
                  <FormControl.Label>
                    {i18n.t('AbpIdentity::PhoneNumber')}
                  </FormControl.Label>
                  <Input
                    ref={phoneNumberRef}
                    onSubmitEditing={() => passwordRef?.current?.focus()}
                    returnKeyType={!editingUser.id ? 'next' : 'default'}
                    onChangeText={formik.handleChange('phoneNumber')}
                    onBlur={formik.handleBlur('phoneNumber')}
                    value={formik.values.phoneNumber}
                  />
                </Stack>
              </FormControl>

              {!editingUser.id ? (
                <FormControl isRequired my="2">
                  <Stack mx="4">
                    <FormControl.Label>
                      {i18n.t('AbpIdentity::Password')}
                    </FormControl.Label>
                    <Input
                      ref={passwordRef}
                      secureTextEntry={!showPassword}
                      onChangeText={formik.handleChange('password')}
                      onBlur={formik.handleBlur('password')}
                      value={formik.values.password}
                      autoCapitalize="none"
                      InputRightElement={
                        <Icon
                          as={Ionicons}
                          size="5"
                          mr="2"
                          name={
                            showPassword ? 'eye-off-outline' : 'eye-outline'
                          }
                          onPress={() => setShowPassword(!showPassword)}
                        />
                      }
                    />
                    <ValidationMessage>
                      {formik.errors.password}
                    </ValidationMessage>
                  </Stack>
                </FormControl>
              ) : null}

              <FormControl my="2">
                <Stack mx="4">
                  <Checkbox
                    isChecked={formik.values.lockoutEnabled}
                    onPress={() =>
                      formik.setFieldValue(
                        'lockoutEnabled',
                        !formik.values.lockoutEnabled
                      )
                    }
                  >
                    {i18n.t('AbpIdentity::DisplayName:LockoutEnabled')}
                  </Checkbox>
                </Stack>
              </FormControl>
            </>
          ) : (
            <UserRoles {...{ editingUser, onChangeRoles }} />
          )}
        </Box>
      </KeyboardAvoidingView>
      <FormButtons
        submit={formik.handleSubmit}
        remove={remove}
        removeMessage={i18n.t('AbpIdentity::UserDeletionConfirmationMessage', {
          0: editingUser.userName,
        })}
        isSubmitDisabled={!formik.isValid}
        isShowRemove={!!editingUser.id && hasRemovePermission}
      />
    </>
  );
}

CreateUpdateUserForm.propTypes = {
  editingUser: PropTypes.object,
  submit: PropTypes.func.isRequired,
  remove: PropTypes.func.isRequired,
};

export default CreateUpdateUserForm;
