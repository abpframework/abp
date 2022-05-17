import i18n from 'i18n-js';
import { Box, Button, FormControl, Input, Text } from 'native-base';
import PropTypes from 'prop-types';
import React, { forwardRef, useState } from 'react';
import { Alert, StyleSheet, View } from 'react-native';
import { getTenant } from '../../api/AccountAPI';
import PersistentStorageActions from '../../store/actions/PersistentStorageActions';
import { createTenantSelector } from '../../store/selectors/PersistentStorageSelectors';
import { connectToRedux } from '../../utils/ReduxConnect';

function TenantBox({
  tenant = {},
  setTenant,
  showTenantSelection,
  toggleTenantSelection,
}) {
  const [tenantName, setTenantName] = useState(tenant.name);

  const findTenant = () => {
    if (!tenantName) {
      setTenant({});
      toggleTenantSelection();
      return;
    }

    getTenant(tenantName).then(({ success, ...data }) => {
      if (!success) {
        Alert.alert(
          i18n.t('AbpUi::Error'),
          i18n.t('AbpUiMultiTenancy::GivenTenantIsNotAvailable', {
            0: tenantName,
          }),
          [{ text: i18n.t('AbpUi::Ok') }]
        );
        return;
      }
      setTenant(data);
      toggleTenantSelection();
    });
  };

  return (
    <>
      <Box
        mb="5"
        px="4"
        w={{
          base: '100%',
        }}
        style={{ flexDirection: 'row' }}
      >
        <View style={{ flex: 1 }}>
          <Text style={styles.title}>
            {i18n.t('AbpUiMultiTenancy::Tenant')}
          </Text>
          <Text style={styles.tenant}>
            {tenant.name
              ? tenant.name
              : i18n.t('AbpUiMultiTenancy::NotSelected')}
          </Text>
        </View>
        <Button
          style={{
            display: !showTenantSelection ? 'flex' : 'none',
          }}
          onPress={() => toggleTenantSelection()}
        >
            {i18n.t('AbpUiMultiTenancy::Switch')}
        </Button>
      </Box>
      {showTenantSelection ? (
        <Box
          px="3"
          w={{
            base: '100%',
          }}
        >
          <FormControl my="2" width={350}>
            <FormControl.Label>
              {i18n.t('AbpUiMultiTenancy::Name')}
            </FormControl.Label>
            <Input
              autoCapitalize="none"
              value={tenantName}
              onChangeText={setTenantName}
            />
          </FormControl>
          <Text style={styles.hint}>
            {i18n.t('AbpUiMultiTenancy::SwitchTenantHint')}
          </Text>
          <View
            style={{ flexDirection: 'row', justifyContent: 'space-between' }}
          >
            <Button
              style={styles.button}
              onPress={() => toggleTenantSelection()}
              variant="outline"
            >
              {i18n.t('AbpAccount::Cancel')}
            </Button>
            <Button style={styles.button} onPress={() => findTenant()}>
              {i18n.t('AbpAccount::Save')}
            </Button>
          </View>
        </Box>
      ) : null}
    </>
  );
}

TenantBox.propTypes = {
  setTenant: PropTypes.func.isRequired,
  showTenantSelection: PropTypes.bool.isRequired,
  toggleTenantSelection: PropTypes.func.isRequired,
  tenant: PropTypes.object.isRequired,
};

const styles = StyleSheet.create({
  button: { marginTop: 20, width: '49%' },

  tenant: { color: '#777' },
  title: {
    marginRight: 10,
    fontSize: 13,
    fontWeight: '600',
    textTransform: 'uppercase',
  },
  hint: { color: '#bbb', textAlign: 'left' },
});

const Forwarded = forwardRef((props, ref) => (
  <TenantBox {...props} forwardedRef={ref} />
));

export default connectToRedux({
  component: Forwarded,
  dispatchProps: {
    setTenant: PersistentStorageActions.setTenant,
  },
  stateProps: (state) => ({
    tenant: createTenantSelector()(state),
  }),
});
