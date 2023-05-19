
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import React from 'react';
import AddIcon from '../components/AddIcon/AddIcon';
import HamburgerIcon from '../components/HamburgerIcon/HamburgerIcon';
import { LocalizationContext } from '../contexts/LocalizationContext';
import CreateUpdateTenantScreen from '../screens/CreateUpdateTenant/CreateUpdateTenantScreen';
import TenantsScreen from '../screens/Tenants/TenantsScreen';

const Stack = createNativeStackNavigator();

export default function TenantsStackNavigator() {
  const { t } = React.useContext(LocalizationContext);

  return (
    <Stack.Navigator initialRouteName="Tenants">
      <Stack.Screen
        name="Tenants"
        component={TenantsScreen}
        options={({ navigation }) => ({
          title: t('AbpTenantManagement::Tenants'),
          headerLeft: () => <HamburgerIcon navigation={navigation} marginLeft={-3} />,
          headerRight: () => <AddIcon onPress={() => navigation.navigate('CreateUpdateTenant')}/>,
        })}
      />
      <Stack.Screen
        name="CreateUpdateTenant"
        component={CreateUpdateTenantScreen}
        options={({ route }) => ({
          title: t(route.params?.tenantId ? 'AbpTenantManagement::Edit' : 'AbpTenantManagement::NewTenant'),
        })}
      />
    </Stack.Navigator>
  );
}
