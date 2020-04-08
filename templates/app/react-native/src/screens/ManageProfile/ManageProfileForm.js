import { Formik } from 'formik';
import i18n from 'i18n-js';
import { Container, Content, Form, Input, InputGroup, Label } from 'native-base';
import PropTypes from 'prop-types';
import React, { useRef } from 'react';
import * as Yup from 'yup';
import FormButtons from '../../components/FormButtons/FormButtons';
import ValidationMessage from '../../components/ValidationMessage/ValidationMessage';

const ValidationSchema = Yup.object().shape({
  userName: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
  email: Yup.string()
    .required('AbpAccount::ThisFieldIsRequired.')
    .email('AbpAccount::ThisFieldIsNotAValidEmailAddress.'),
});

function ManageProfileForm({ editingUser = {}, submit, cancel }) {
  const usernameRef = useRef();
  const nameRef = useRef();
  const surnameRef = useRef();
  const emailRef = useRef();
  const phoneNumberRef = useRef();

  const onSubmit = values => {
    submit({
      ...editingUser,
      ...values,
    });
  };

  return (
    <Formik
      enableReinitialize
      validationSchema={ValidationSchema}
      initialValues={{
        ...editingUser,
      }}
      onSubmit={values => onSubmit(values)}>
      {({ handleChange, handleBlur, handleSubmit, values, errors, isValid }) => (
        <>
          <Container>
            <Content px20>
              <Form>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpIdentity::UserName')}*</Label>
                  <Input
                    ref={usernameRef}
                    onSubmitEditing={() => nameRef.current._root.focus()}
                    returnKeyType="next"
                    onChangeText={handleChange('userName')}
                    onBlur={handleBlur('userName')}
                    value={values.userName}
                    abpInput
                  />
                </InputGroup>
                <ValidationMessage>{errors.userName}</ValidationMessage>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpIdentity::DisplayName:Name')}</Label>
                  <Input
                    abpInput
                    ref={nameRef}
                    onSubmitEditing={() => surnameRef.current._root.focus()}
                    returnKeyType="next"
                    onChangeText={handleChange('name')}
                    onBlur={handleBlur('name')}
                    value={values.name}
                  />
                </InputGroup>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpIdentity::DisplayName:Surname')}</Label>
                  <Input
                    abpInput
                    ref={surnameRef}
                    onSubmitEditing={() => phoneNumberRef.current._root.focus()}
                    returnKeyType="next"
                    onChangeText={handleChange('surname')}
                    onBlur={handleBlur('surname')}
                    value={values.surname}
                  />
                </InputGroup>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpIdentity::PhoneNumber')}</Label>
                  <Input
                    abpInput
                    ref={phoneNumberRef}
                    onSubmitEditing={() => emailRef.current._root.focus()}
                    returnKeyType="next"
                    onChangeText={handleChange('phoneNumber')}
                    onBlur={handleBlur('phoneNumber')}
                    value={values.phoneNumber}
                  />
                </InputGroup>
                <InputGroup abpInputGroup>
                  <Label abpLabel>{i18n.t('AbpIdentity::EmailAddress')}*</Label>
                  <Input
                    abpInput
                    ref={emailRef}
                    returnKeyType="done"
                    onSubmitEditing={handleSubmit}
                    onChangeText={handleChange('email')}
                    onBlur={handleBlur('email')}
                    value={values.email}
                  />
                </InputGroup>
                <ValidationMessage>{errors.email}</ValidationMessage>
              </Form>
            </Content>
          </Container>
          <FormButtons submit={handleSubmit} cancel={cancel} isSubmitDisabled={!isValid} />
        </>
      )}
    </Formik>
  );
}

ManageProfileForm.propTypes = {
  editingUser: PropTypes.object.isRequired,
  submit: PropTypes.func.isRequired,
  cancel: PropTypes.func.isRequired,
};

export default ManageProfileForm;
