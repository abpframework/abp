import { createNativeStackNavigator } from '@react-navigation/native-stack';
import React from 'react';
import AddIcon from '../components/AddIcon/AddIcon';
import HamburgerIcon from '../components/HamburgerIcon/HamburgerIcon';
import { LocalizationContext } from '../contexts/LocalizationContext';
import CreateUpdateUserScreen from '../screens/CreateUpdateUser/CreateUpdateUserScreen';
import UsersScreen from '../screens/Users/UsersScreen';

const Stack = createNativeStackNavigator();

export default function UsersStackNavigator() {
  const { t } = React.useContext(LocalizationContext);

  return (
    <Stack.Navigator initialRouteName="Users">
      <Stack.Screen
        name="Users"
        component={UsersScreen}
        options={({ navigation }) => ({
          title: t('AbpIdentity::Users'),
          headerLeft: () => <HamburgerIcon navigation={navigation} marginLeft={-3} />,
          headerRight: () => <AddIcon onPress={() => navigation.navigate('CreateUpdateUser')}/>,
        })}
      />
      <Stack.Screen
        name="CreateUpdateUser"
        component={CreateUpdateUserScreen}
        options={({ route }) => ({
          title: t(route.params?.userId ? 'AbpIdentity::Edit' : 'AbpIdentity::NewUser'),
        })}
      />
    </Stack.Navigator>
  );
}
