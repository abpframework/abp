import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import i18n from 'i18n-js';
import SettingsScreen from '../screens/Settings/SettingsScreen';
import ChangePasswordScreen from '../screens/ChangePassword/ChangePasswordScreen';
import ManageProfileScreen from '../screens/ManageProfile/ManageProfileScreen';
import MenuIcon from '../components/MenuIcon/MenuIcon';
import { LocalizationContext } from '../contexts/LocalizationContext';

const Stack = createStackNavigator();

export default function SettingsStackNavigator() {
  const { t } = React.useContext(LocalizationContext);

  return (
    <Stack.Navigator initialRouteName="Settings">
      <Stack.Screen
        name="Settings"
        component={SettingsScreen}
        options={({ navigation }) => ({
          headerLeft: () => <MenuIcon onPress={() => navigation.openDrawer()} />,
          title: t('AbpSettingManagement::Settings'),
        })}
      />
      <Stack.Screen
        name="ChangePassword"
        component={ChangePasswordScreen}
        options={{
          title: i18n.t('AbpUi::ChangePassword'),
        }}
      />
      <Stack.Screen
        name="ManageProfile"
        component={ManageProfileScreen}
        options={{
          title: i18n.t('AbpAccount::MyAccount'),
        }}
      />
    </Stack.Navigator>
  );
}
