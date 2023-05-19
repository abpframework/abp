import { Ionicons } from '@expo/vector-icons';
import Constants from 'expo-constants';
import i18n from 'i18n-js';
import { List, Text, View } from 'native-base';
import PropTypes from 'prop-types';
import React from 'react';
import { Image, StyleSheet } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { withPermission } from '../../hocs/PermissionHOC';

const screens = {
  HomeStack: { label: '::Menu:Home', iconName: 'home' },
  UsersStack: {
    label: 'AbpIdentity::Users',
    iconName: 'people',
    requiredPolicy: 'AbpIdentity.Users',
  },
  TenantsStack: {
    label: 'AbpTenantManagement::Tenants',
    iconName: 'book-outline',
    requiredPolicy: 'AbpTenantManagement.Tenants',
  },
  SettingsStack: { label: 'AbpSettingManagement::Settings', iconName: 'cog' },
};

const ListItemWithPermission = withPermission(List.Item);

function DrawerContent({
  navigation,
  state: { routeNames, index: currentScreenIndex },
}) {
  const navigate = (screen) => {
    navigation.navigate(screen);
    navigation.closeDrawer();
  };

  return (
    <View style={styles.container}>
      <SafeAreaView
        style={styles.container}
        forceInset={{ top: 'always', horizontal: 'never' }}
      >
        <View style={styles.headerView}>
          <Image
            style={styles.logo}
            source={require('../../../assets/logo.png')}
          />
        </View>
        <List my={2} py={0}>
          {routeNames.map((name) => (
            <ListItemWithPermission
              key={name}
              policyKey={screens[name].requiredPolicy}
              bg={name === routeNames[currentScreenIndex] ? 'primary.400': 'transparent'}
              onPress={() => navigate(name)}
              my="0"
            >
              <List.Icon as={Ionicons} name={screens[name].iconName} size="7"/>
              {i18n.t(screens[name].label)}
            </ListItemWithPermission>
          ))}
        </List>
      </SafeAreaView>
      <View style={styles.footer}>
        <Text note style={styles.copyRight}>
          © MyProjectName
        </Text>
        <Text note style={styles.version}>
          v{Constants.manifest.version}
        </Text>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flexGrow: 1,
  },
  logo: {
    marginTop: 20,
    marginBottom: 15,
  },
  headerView: {
    alignItems: 'center',
  },
  footer: {
    backgroundColor: '#eee',
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  copyRight: {
    margin: 15,
  },
  version: {
    margin: 15,
  },
});

DrawerContent.propTypes = {
  state: PropTypes.object.isRequired,
};

export default DrawerContent;
