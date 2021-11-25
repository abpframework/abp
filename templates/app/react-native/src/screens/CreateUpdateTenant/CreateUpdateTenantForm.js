import { Formik } from 'formik';
import i18n from 'i18n-js';
import { Container, Content, Input, InputGroup, Label, Item, Icon } from 'native-base';
import PropTypes from 'prop-types';
import React, { useRef, useState } from 'react';
import { StyleSheet } from 'react-native';
import * as Yup from 'yup';
import FormButtons from '../../components/FormButtons/FormButtons';
import ValidationMessage from '../../components/ValidationMessage/ValidationMessage';
import { usePermission } from '../../hooks/UsePermission';

const validations = {
  name: Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
};

function CreateUpdateTenantForm({ editingTenant = {}, submit, remove }) {
  const tenantNameRef = useRef();
  const adminEmailRef = useRef();
  const adminPasswordRef = useRef();

  const [showAdminPassword, setShowAdminPassword] = useState(false);
  const hasRemovePermission = usePermission('AbpTenantManagement.Tenants.Delete');

  const onSubmit = values => {
    submit({
      ...editingTenant,
      ...values,
    });
  };

  const adminEmailAddressValidation = Yup.lazy(() =>
    Yup.string()
      .required('AbpAccount::ThisFieldIsRequired.')
      .email('AbpAccount::ThisFieldIsNotAValidEmailAddress.'),
  );

  const adminPasswordValidation = Yup.lazy(() =>
    Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
  );

  return (
    <Formik
      enableReinitialize
      validationSchema={Yup.object().shape({
        ...validations,
        ...(!editingTenant.id && {
          adminEmailAddress: adminEmailAddressValidation,
          adminPassword: adminPasswordValidation,
        }),
      })}
      initialValues={{
        ...editingTenant,
      }}
      onSubmit={values => onSubmit(values)}>
      {({ handleChange, handleBlur, handleSubmit, values, errors, isValid }) => (
        <>
          <Container style={styles.container}>
            <Content px20>
              <InputGroup abpInputGroup>
                <Label abpLabel>{i18n.t('AbpTenantManagement::TenantName')}</Label>
                <Input
                  abpInput
                  ref={tenantNameRef}
                  onSubmitEditing={() =>
                    !editingTenant.id ? adminEmailRef.current._root.focus() : null
                  }
                  returnKeyType={!editingTenant.id ? 'next' : 'done'}
                  onChangeText={handleChange('name')}
                  onBlur={handleBlur('name')}
                  value={values.name}
                  autoCapitalize = 'none'
                />
              </InputGroup>
              <ValidationMessage>{errors.name}</ValidationMessage>
              {!editingTenant.id ? (
                <>
                  <InputGroup abpInputGroup>
                    <Label abpLabel>
                      {i18n.t('AbpTenantManagement::DisplayName:AdminEmailAddress')}
                    </Label>
                    <Input
                      abpInput
                      ref={adminEmailRef}
                      onSubmitEditing={() => adminPasswordRef.current._root.focus()}
                      returnKeyType="next"
                      onChangeText={handleChange('adminEmailAddress')}
                      onBlur={handleBlur('adminEmailAddress')}
                      value={values.adminEmailAddress}
                      autoCapitalize = 'none'
                    />
                  </InputGroup>
                  <ValidationMessage>{errors.adminEmailAddress}</ValidationMessage>
                  <InputGroup abpInputGroup>
                    <Label abpLabel>
                      {i18n.t('AbpTenantManagement::DisplayName:AdminPassword')}
                    </Label>
                    <Item abpInput>
                      <Input
                        ref={adminPasswordRef}
                        returnKeyType="done"
                        secureTextEntry={!showAdminPassword}
                        onChangeText={handleChange('adminPassword')}
                        onBlur={handleBlur('adminPassword')}
                        value={values.adminPassword}
                        autoCapitalize = 'none'
                      />
                      <Icon
                        name={showAdminPassword ? 'eye-off' : 'eye'}
                        onPress={() => setShowAdminPassword(!showAdminPassword)}
                      />
                    </Item>
                  </InputGroup>
                  <ValidationMessage>{errors.adminPassword}</ValidationMessage>
                </>
              ) : null}
            </Content>
          </Container>
          <FormButtons
            submit={handleSubmit}
            remove={remove}
            removeMessage={i18n.t('AbpTenantManagement::TenantDeletionConfirmationMessage', {
              0: editingTenant.name,
            })}
            isSubmitDisabled={!isValid}
            isShowRemove={!!editingTenant.id && hasRemovePermission}
          />
        </>
      )}
    </Formik>
  );
}

CreateUpdateTenantForm.propTypes = {
  editingTenant: PropTypes.object,
  submit: PropTypes.func.isRequired,
  remove: PropTypes.func.isRequired,
};

const styles = StyleSheet.create({
  container: {
    marginBottom: 50,
  },
});

export default CreateUpdateTenantForm;
