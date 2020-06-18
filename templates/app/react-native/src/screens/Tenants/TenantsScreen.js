import React from 'react';
import i18n from 'i18n-js';
import { ListItem, Text, Icon, Left, Body, Fab } from 'native-base';
import { StyleSheet } from 'react-native';
import { getTenants } from '../../api/TenantManagementAPI';
import { activeTheme } from '../../theme/variables';
import DataList from '../../components/DataList/DataList';
import { withPermission } from '../../hocs/PermissionHOC';

const CreateTenantButtonWithPermission = withPermission(Fab, 'AbpTenantManagement.Tenants.Create');

function TenantsScreen({ navigation }) {
  return (
    <>
      <DataList
        navigation={navigation}
        fetchFn={getTenants}
        render={tenant => (
          <ListItem avatar onPress={() => navigateToCreateUpdateTenantScreen(navigation, tenant)}>
            <Left style={styles.listItem}>
              <Body>
                <Text>{tenant.name}</Text>
              </Body>
            </Left>
          </ListItem>
        )}
      />

      <CreateTenantButtonWithPermission
        style={{ backgroundColor: activeTheme.brandPrimary }}
        position="bottomRight"
        onPress={() => navigation.navigate('CreateUpdateTenant')}>
        <Icon name="add" />
      </CreateTenantButtonWithPermission>
    </>
  );
}

const navigateToCreateUpdateTenantScreen = (navigation, tenant = {}) => {
  navigation.navigate('CreateUpdateTenant', {
    tenantId: tenant.id,
  });
};

const styles = StyleSheet.create({
  listItem: {
    alignItems: 'center',
    marginVertical: 3,
    marginLeft: -15,
    paddingTop: 5,
    paddingBottom: 5,
    paddingLeft: 15,
    backgroundColor: '#f9f9f9',
    width: '110%',
    minHeight: 50,
  },
});

export default TenantsScreen;
