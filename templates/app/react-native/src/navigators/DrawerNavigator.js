import { createDrawerNavigator } from '@react-navigation/drawer';
import React from 'react';
import DrawerContent from '../components/DrawerContent/DrawerContent';
import HamburgerIcon from '../components/HamburgerIcon/HamburgerIcon';
import { LocalizationContext } from '../contexts/LocalizationContext';
import HomeStackNavigator from './HomeNavigator';
import SettingsStackNavigator from './SettingsNavigator';
import TenantsStackNavigator from './TenantsNavigator';
import UsersStackNavigator from './UsersNavigator';

const Drawer = createDrawerNavigator();

export default function DrawerNavigator() {
  const { t } = React.useContext(LocalizationContext);

  return (
    <Drawer.Navigator initialRouteName="Home" drawerContent={DrawerContent}>
      <Drawer.Screen
        name="HomeStack"
        component={HomeStackNavigator}
        options={({ navigation }) => ({
          title: t('::Menu:Home'),
          headerLeft: () => <HamburgerIcon navigation={navigation} />,
        })}
      />
      <Drawer.Screen
        name="TenantsStack"
        component={TenantsStackNavigator}
        options={{ header: () => null }}
      />
      <Drawer.Screen
        name="UsersStack"
        component={UsersStackNavigator}
        options={{ header: () => null }}
      />
      <Drawer.Screen
        name="SettingsStack"
        component={SettingsStackNavigator}
        options={{ header: () => null }}
      />
    </Drawer.Navigator>
  );
}
