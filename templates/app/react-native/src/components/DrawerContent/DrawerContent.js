import { Text, View, List, ListItem, Left, Icon, Body } from 'native-base';
import React from 'react';
import { Image, StyleSheet } from 'react-native';
import SafeAreaView from 'react-native-safe-area-view';
import i18n from 'i18n-js';
import PropTypes from 'prop-types';
import Constants from 'expo-constants';
import { withPermission } from '../../hocs/PermissionHOC';

const screens = {
  Home: { label: '::Menu:Home', iconName: 'home' },
  Users: {
    label: 'AbpIdentity::Users',
    iconName: 'contacts',
    requiredPolicy: 'AbpIdentity.Users',
  },
  Tenants: {
    label: 'AbpTenantManagement::Tenants',
    iconName: 'people',
    requiredPolicy: 'AbpTenantManagement.Tenants',
  },
  Settings: { label: 'AbpSettingManagement::Settings', iconName: 'cog' },
};

const ListItemWithPermission = withPermission(ListItem);

function DrawerContent({ navigation, state: { routeNames, index: currentScreenIndex } }) {
  const navigate = screen => {
    navigation.navigate(screen);
    navigation.closeDrawer();
  };

  return (
    <View style={styles.container}>
      <SafeAreaView style={styles.container} forceInset={{ top: 'always', horizontal: 'never' }}>
        <View style={styles.headerView}>
          <Image style={styles.logo} source={require('../../../assets/logo.png')} />
        </View>
        <List
          dataArray={routeNames}
          keyExtractor={item => item}
          renderRow={name => (
            <ListItemWithPermission
              icon
              key={name}
              policyKey={screens[name].requiredPolicy}
              selected={name === routeNames[currentScreenIndex]}
              onPress={() => navigate(name)}
              style={{
                ...styles.navItem,
                backgroundColor: name === routeNames[currentScreenIndex] ? '#38003c' : '#f2f2f2',
              }}>
              <Left>
                <Icon
                  dark={name !== routeNames[currentScreenIndex]}
                  light={name === routeNames[currentScreenIndex]}
                  name={screens[name].iconName}
                />
              </Left>
              <Body style={{ borderBottomWidth: 0 }}>
                <Text
                  style={{
                    color: name === routeNames[currentScreenIndex] ? '#fff' : '#000',
                  }}>
                  {i18n.t(screens[name].label)}
                </Text>
              </Body>
            </ListItemWithPermission>
          )}
        />
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
    borderBottomWidth: 1,
    borderColor: '#eee',
    alignItems: 'center',
  },
  navItem: {
    marginLeft: 0,
    marginBottom: 3,
    paddingLeft: 10,
    width: '100%',
    backgroundColor: '#f2f2f2',
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
