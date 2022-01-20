import { Ionicons } from '@expo/vector-icons';
import { useFocusEffect } from '@react-navigation/native';
import i18n from 'i18n-js';
import { Avatar, Button, Divider, FormControl, List, Select, Stack, Text, View } from 'native-base';
import PropTypes from 'prop-types';
import React, { useCallback, useState } from 'react';
import { getProfileDetail } from '../../api/IdentityAPI';
import AppActions from '../../store/actions/AppActions';
import {
  createLanguageSelector,
  createLanguagesSelector
} from '../../store/selectors/AppSelectors';
import { createTenantSelector } from '../../store/selectors/PersistentStorageSelectors';
import { connectToRedux } from '../../utils/ReduxConnect';

function SettingsScreen({
  navigation,
  language,
  languages,
  setLanguageAsync,
  logoutAsync,
  tenant = {},
}) {
  const [user, setUser] = useState({});

  const fetchUser = () => {
    getProfileDetail().then(data => {
      setUser(data || {});
    });
  };

  useFocusEffect(
    useCallback(() => {
      fetchUser();
    }, []),
  );

  return (
    <View>
      <List px="0" py="0" borderWidth="0">
        <List.Item
          style={{ backgroundColor: '#fff' }}
          onPress={() => navigation.navigate('ManageProfile')}>
          <View
            style={{
              flexDirection: 'row',
              justifyContent: 'space-between',
              alignItems: 'center',
              width: '100%',
            }}>
            <View style={{ flexDirection: 'row', alignItems: 'center' }}>
              <Avatar ml="2" source={require('../../../assets/avatar.png')} />
              <Text ml="2">
                {tenant.name ? `${tenant.name}/` : ''}
                {user.userName ? `${user.userName}` : ''}
              </Text>
            </View>

            <List.Icon as={Ionicons} name="arrow-forward" size="5" />
          </View>
        </List.Item>
        <Divider thickness={5} />
        <List.Item
          style={{ backgroundColor: '#fff' }}
          onPress={() => navigation.navigate('ChangePassword')}>
          <View
            style={{
              flexDirection: 'row',
              justifyContent: 'space-between',
              alignItems: 'center',
              width: '100%',
            }}>
            <Text ml="2">{i18n.t('AbpUi::ChangePassword')}</Text>

            <List.Icon as={Ionicons} name="arrow-forward" size="5" />
          </View>
        </List.Item>
        <Divider thickness={5} />
        <List.Item style={{ backgroundColor: '#fff' }}>
          <FormControl my="2">
            <Stack mx="2">
              <FormControl.Label>{i18n.t('AbpUi::Language')}</FormControl.Label>
              <Select
                mode="dropdown"
                onValueChange={setLanguageAsync}
                selectedValue={language.cultureName}>
                {languages.map(lang => (
                  <Select.Item
                    label={lang.displayName}
                    value={lang.cultureName}
                    key={lang.cultureName}
                  />
                ))}
              </Select>
            </Stack>
          </FormControl>
        </List.Item>
        <Divider thickness={10} />
        <Button
          bg="danger.500"
          style={{ borderRadius: 0 }}
          onPress={() => {
            logoutAsync();
          }}>
          {i18n.t('AbpAccount::Logout')}
        </Button>
      </List>
    </View>
  );
}

SettingsScreen.propTypes = {
  setLanguageAsync: PropTypes.func.isRequired,
  logoutAsync: PropTypes.func.isRequired,
  language: PropTypes.object.isRequired,
  languages: PropTypes.array.isRequired,
  tenant: PropTypes.object.isRequired,
};

export default connectToRedux({
  component: SettingsScreen,
  stateProps: state => ({
    languages: createLanguagesSelector()(state),
    language: createLanguageSelector()(state),
    tenant: createTenantSelector()(state),
  }),
  dispatchProps: {
    setLanguageAsync: AppActions.setLanguageAsync,
    logoutAsync: AppActions.logoutAsync,
  },
});
