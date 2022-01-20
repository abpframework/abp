import { Ionicons } from '@expo/vector-icons';
import { useFormik } from 'formik';
import i18n from 'i18n-js';
import { Box, FormControl, Icon, Input, Stack } from 'native-base';
import PropTypes from 'prop-types';
import React, { useRef, useState } from 'react';
import * as Yup from 'yup';
import { FormButtons } from '../../components/FormButtons';
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

  const adminEmailAddressValidation = Yup.lazy(() =>
    Yup.string()
      .required('AbpAccount::ThisFieldIsRequired.')
      .email('AbpAccount::ThisFieldIsNotAValidEmailAddress.'),
  );

  const adminPasswordValidation = Yup.lazy(() =>
    Yup.string().required('AbpAccount::ThisFieldIsRequired.'),
  );

  const onSubmit = values => {
    submit({
      ...editingTenant,
      ...values,
    });
  };

  const formik = useFormik({
    enableReinitialize: true,
    validationSchema: Yup.object().shape({
      ...validations,
      ...(!editingTenant.id && {
        adminEmailAddress: adminEmailAddressValidation,
        adminPassword: adminPasswordValidation,
      }),
    }),
    initialValues: {
      lockoutEnabled: false,
      twoFactorEnabled: false,
      ...editingTenant,
    },
    onSubmit,
  });

  return (
    <>
      <Box w={{ base: '100%' }} px="3">
        <FormControl isRequired my="2">
          <Stack mx="4">
            <FormControl.Label>{i18n.t('AbpTenantManagement::TenantName')}</FormControl.Label>
            <Input
              ref={tenantNameRef}
              onChangeText={formik.handleChange('name')}
              onBlur={formik.handleBlur('name')}
              value={formik.values.name}
              autoCapitalize="none"
              returnKeyType="next"
            />
            <ValidationMessage>{formik.errors.name}</ValidationMessage>
          </Stack>
        </FormControl>

        {!editingTenant.id ? (
          <>
            <FormControl isRequired my="2">
              <Stack mx="4">
                <FormControl.Label>
                  {i18n.t('AbpTenantManagement::DisplayName:AdminEmailAddress')}
                </FormControl.Label>
                <Input
                  ref={adminEmailRef}
                  onChangeText={formik.handleChange('adminEmailAddress')}
                  onBlur={formik.handleBlur('adminEmailAddress')}
                  value={formik.values.adminEmailAddress}
                  autoCapitalize="none"
                  onSubmitEditing={() => adminPasswordRef?.current?.focus()}
                  returnKeyType="next"
                />
                <ValidationMessage>{formik.errors.adminEmailAddress}</ValidationMessage>
              </Stack>
            </FormControl>

            <FormControl isRequired my="2">
              <Stack mx="4">
                <FormControl.Label>
                  {i18n.t('AbpTenantManagement::DisplayName:AdminPassword')}
                </FormControl.Label>
                <Input
                  ref={adminPasswordRef}
                  returnKeyType="done"
                  secureTextEntry={!showAdminPassword}
                  onChangeText={formik.handleChange('adminPassword')}
                  onBlur={formik.handleBlur('adminPassword')}
                  value={formik.values.adminPassword}
                  autoCapitalize="none"
                  InputRightElement={
                    <Icon
                      as={Ionicons}
                      size="5"
                      mr="2"
                      name={showAdminPassword ? 'eye-off-outline' : 'eye-outline'}
                      onPress={() => setShowAdminPassword(!showAdminPassword)}
                    />
                  }
                />
                <ValidationMessage>{formik.errors.adminPassword}</ValidationMessage>
              </Stack>
            </FormControl>
          </>
        ) : null}
      </Box>
      <FormButtons
        submit={formik.handleSubmit}
        remove={remove}
        removeMessage={i18n.t('AbpTenantManagement::TenantDeletionConfirmationMessage', {
          0: editingTenant.name,
        })}
        isSubmitDisabled={!formik.isValid}
        isShowRemove={!!editingTenant.id && hasRemovePermission}
      />
    </>
  );
}

CreateUpdateTenantForm.propTypes = {
  editingTenant: PropTypes.object,
  submit: PropTypes.func.isRequired,
  remove: PropTypes.func.isRequired,
};

export default CreateUpdateTenantForm;
