import { Formik } from 'formik';
import i18n from 'i18n-js';
import { Container, Content, Input, InputGroup, Label } from 'native-base';
import PropTypes from 'prop-types';
import React, { useRef } from 'react';
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

  const hasRemovePermission = usePermission('AbpTenantManagement.Tenants.Delete');

  const onSubmit = values => {
    submit({
      ...editingTenant,
      ...values,
    });
  };

  return (
    <Formik
      enableReinitialize
      validationSchema={Yup.object().shape({
        ...validations,
      })}
      initialValues={{
        lockoutEnabled: false,
        twoFactorEnabled: false,
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
                  onChangeText={handleChange('name')}
                  onBlur={handleBlur('name')}
                  value={values.name}
                />
              </InputGroup>
              <ValidationMessage>{errors.name}</ValidationMessage>
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
