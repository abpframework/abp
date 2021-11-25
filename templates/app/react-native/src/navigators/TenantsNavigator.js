import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import TenantsScreen from '../screens/Tenants/TenantsScreen';
import CreateUpdateTenantScreen from '../screens/CreateUpdateTenant/CreateUpdateTenantScreen';
import MenuIcon from '../components/MenuIcon/MenuIcon';
import { LocalizationContext } from '../contexts/LocalizationContext';

const Stack = createStackNavigator();

export default function TenantsStackNavigator() {
  const { t } = React.useContext(LocalizationContext);

  return (
    <Stack.Navigator initialRouteName="Tenants">
      <Stack.Screen
        name="Tenants"
        component={TenantsScreen}
        options={({ navigation }) => ({
          headerLeft: () => <MenuIcon onPress={() => navigation.openDrawer()} />,
          title: t('AbpTenantManagement::Tenants'),
        })}
      />
      <Stack.Screen
        name="CreateUpdateTenant"
        component={CreateUpdateTenantScreen}
        options={({ route }) => ({
          title: t(
            route.params?.tenantId ? 'AbpTenantManagement::Edit' : 'AbpTenantManagement::NewTenant',
          ),
        })}
      />
    </Stack.Navigator>
  );
}
