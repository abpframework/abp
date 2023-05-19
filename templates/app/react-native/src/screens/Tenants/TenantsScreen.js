import { Box, HStack, Pressable, Text } from 'native-base';
import React from 'react';
import { getTenants } from '../../api/TenantManagementAPI';
import DataList from '../../components/DataList/DataList';
import { LocalizationContext } from '../../contexts/LocalizationContext';

function TenantsScreen({ navigation }) {
  const { t } = React.useContext(LocalizationContext);

  return (
    <Box
      w={{
        base: '100%',
        md: '25%',
      }}
    >
      <DataList
        navigation={navigation}
        fetchFn={getTenants}
        render={({ item }) => (
          <Pressable
            onPress={() => navigateToCreateUpdateTenantScreen(navigation, item)}
          >
            <Box
              borderBottomWidth="1"
              borderColor="coolGray.200"
              pl="2"
              pr="5"
              py="2"
            >
              <HStack space={1}>
                <Text color="coolGray.500">{t('AbpTenantManagement::TenantName')}:</Text>
                <Text color="coolGray.800" bold>
                  {item.name}
                </Text>
              </HStack>
            </Box>
          </Pressable>
        )}
      />
    </Box>
  );
}

const navigateToCreateUpdateTenantScreen = (navigation, tenant = {}) => {
  navigation.navigate('CreateUpdateTenant', {
    tenantId: tenant.id,
  });
};

export default TenantsScreen;
