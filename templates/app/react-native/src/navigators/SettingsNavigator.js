import { createNativeStackNavigator } from '@react-navigation/native-stack';
import i18n from 'i18n-js';
import React from 'react';
import HamburgerIcon from '../components/HamburgerIcon/HamburgerIcon';
import { LocalizationContext } from '../contexts/LocalizationContext';
import ChangePasswordScreen from '../screens/ChangePassword/ChangePasswordScreen';
import ManageProfileScreen from '../screens/ManageProfile/ManageProfileScreen';
import SettingsScreen from '../screens/Settings/SettingsScreen';

const Stack = createNativeStackNavigator();

export default function SettingsStackNavigator() {
  const { t } = React.useContext(LocalizationContext);

  return (
    <Stack.Navigator initialRouteName="Settings">
      <Stack.Screen
        name="Settings"
        component={SettingsScreen}
        options={({ navigation }) => ({
          headerLeft: () => <HamburgerIcon navigation={navigation} marginLeft={-3} />,
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
