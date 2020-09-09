import { createStackNavigator } from '@react-navigation/stack';
import React from 'react';
import { LocalizationContext } from '../contexts/LocalizationContext';
import LoginScreen from '../screens/Login/LoginScreen';

const Stack = createStackNavigator();

export default function AuthStackNavigator() {
  const { t } = React.useContext(LocalizationContext);

  return (
    <Stack.Navigator initialRouteName="Login">
      <Stack.Screen
        name="Login"
        component={LoginScreen}
        options={() => ({
          title: t('AbpAccount::Login'),
        })}
      />
    </Stack.Navigator>
  );
}
