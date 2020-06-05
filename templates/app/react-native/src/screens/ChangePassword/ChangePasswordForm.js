import { Formik } from 'formik';
import i18n from 'i18n-js';
import { Container, Content, Form, Input, InputGroup, Item, Icon, Label } from 'native-base';
import PropTypes from 'prop-types';
import React, { useRef, useState } from 'react';
import * as Yup from 'yup';
import FormButtons from '../../components/FormButtons/FormButtons';
import ValidationMessage from '../../components/ValidationMessage/ValidationMessage';

const ValidationSchema = Yup.object().shape({
  currentPassword: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
  newPassword: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
});

function ChangePasswordForm({ submit, cancel }) {
  const [showCurrentPassword, setShowCurrentPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);

  const currentPasswordRef = useRef();
  const newPasswordRef = useRef();

  const onSubmit = values => {
    submit({
      ...values,
      newPasswordConfirm: values.newPassword,
    });
  };

  return (
    <Formik
      enableReinitialize
      validationSchema={ValidationSchema}
      initialValues={{
        currentPassword: '',
        newPassword: '',
      }}
      onSubmit={values => onSubmit(values)}>
      {({ handleChange, handleBlur, handleSubmit, values, errors, isValid }) => (
        <>
          <Container>
            <Content px20>
              <Form>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpIdentity::DisplayName:CurrentPassword')}</Label>
                  <Item abpInput>
                    <Input
                      ref={currentPasswordRef}
                      onSubmitEditing={() => newPasswordRef.current._root.focus()}
                      returnKeyType="next"
                      onChangeText={handleChange('currentPassword')}
                      onBlur={handleBlur('currentPassword')}
                      value={values.currentPassword}
                      textContentType="password"
                      secureTextEntry={!showCurrentPassword}
                    />
                    <Icon
                      active
                      name={showCurrentPassword ? 'eye-off' : 'eye'}
                      onPress={() => setShowCurrentPassword(!showCurrentPassword)}
                    />
                  </Item>
                </InputGroup>
                <ValidationMessage>{errors.currentPassword}</ValidationMessage>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpIdentity::DisplayName:NewPassword')}</Label>
                  <Item abpInput>
                    <Input
                      ref={newPasswordRef}
                      returnKeyType="done"
                      onSubmitEditing={handleSubmit}
                      onChangeText={handleChange('newPassword')}
                      onBlur={handleBlur('newPassword')}
                      value={values.newPassword}
                      textContentType="newPassword"
                      secureTextEntry={!showNewPassword}
                    />
                    <Icon
                      name={showNewPassword ? 'eye-off' : 'eye'}
                      onPress={() => setShowNewPassword(!showNewPassword)}
                    />
                  </Item>
                </InputGroup>
                <ValidationMessage>{errors.newPassword}</ValidationMessage>
              </Form>
            </Content>
          </Container>
          <FormButtons submit={handleSubmit} cancel={cancel} isSubmitDisabled={!isValid} />
        </>
      )}
    </Formik>
  );
}

ChangePasswordForm.propTypes = {
  submit: PropTypes.func.isRequired,
  cancel: PropTypes.func.isRequired,
};

export default ChangePasswordForm;
