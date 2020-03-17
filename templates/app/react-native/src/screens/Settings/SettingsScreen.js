import i18n from 'i18n-js';
import {
  Body,
  Button,
  Icon,
  Label,
  Left,
  List,
  ListItem,
  Picker,
  Right,
  Text,
  Thumbnail,
} from 'native-base';
import PropTypes from 'prop-types';
import React, { useCallback, useState } from 'react';
import { View } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { getProfileDetail } from '../../api/IdentityAPI';
import AppActions from '../../store/actions/AppActions';
import {
  createLanguageSelector,
  createLanguagesSelector,
} from '../../store/selectors/AppSelectors';
import { connectToRedux } from '../../utils/ReduxConnect';
import { createTenantSelector } from '../../store/selectors/PersistentStorageSelectors';

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
      <List>
        <ListItem itemDivider />
        <ListItem
          noIndent
          style={{ backgroundColor: '#fff' }}
          onPress={() => navigation.navigate('ManageProfile')}>
          <Left style={{ alignItems: 'center' }}>
            <Thumbnail source={require('../../../assets/avatar.png')} />
            <Body>
              <Text>
                {tenant.name ? `${tenant.name} / ` : ''}
                {user.userName ? `${user.userName}` : ''}
              </Text>
              <Text note>{user.email}</Text>
            </Body>
          </Left>

          <Right>
            <Icon active name="arrow-forward" />
          </Right>
        </ListItem>
        <ListItem itemDivider />
        <ListItem
          noIndent
          icon
          style={{ backgroundColor: '#fff' }}
          onPress={() => navigation.navigate('ChangePassword')}>
          <Body>
            <Text>{i18n.t('AbpUi::ChangePassword')}</Text>
          </Body>
          <Right>
            <Icon active name="arrow-forward" />
          </Right>
        </ListItem>
        <ListItem itemDivider />
        <ListItem itemDivider>
          <Label abpLabel style={{ marginBottom: 0 }}>
            {i18n.t('AbpUi::Language')}
          </Label>
        </ListItem>
        <ListItem noIndent icon style={{ backgroundColor: '#fff' }}>
          <Body>
            <Picker
              mode="dropdown"
              iosHeader={i18n.t('AbpUi::Language')}
              iosIcon={<Icon active name="arrow-down" />}
              onValueChange={value => setLanguageAsync(value)}
              selectedValue={language.cultureName}
              textStyle={{ paddingLeft: 0 }}>
              {languages.map(lang => (
                <Picker.Item
                  label={lang.displayName}
                  value={lang.cultureName}
                  key={lang.cultureName}
                />
              ))}
            </Picker>
          </Body>
        </ListItem>
        <ListItem itemDivider />
        <Button
          abpButton
          danger
          style={{ borderRadius: 0 }}
          onPress={() => {
            logoutAsync();
          }}>
          <Text>{i18n.t('AbpAccount::Logout')}</Text>
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
