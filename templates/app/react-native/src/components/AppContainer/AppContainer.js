import { Ionicons } from '@expo/vector-icons';
import * as Font from 'expo-font';
import i18n from 'i18n-js';
import PropTypes from 'prop-types';
import React, { useEffect, useState, useMemo } from 'react';
import { Platform, StatusBar } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { Root } from 'native-base';
import Loading from '../Loading/Loading';
import { connectToRedux } from '../../utils/ReduxConnect';
import { createLanguageSelector } from '../../store/selectors/AppSelectors';
import { createTokenSelector } from '../../store/selectors/PersistentStorageSelectors';
import AppActions from '../../store/actions/AppActions';
import PersistentStorageActions from '../../store/actions/PersistentStorageActions';
import { LocalizationContext } from '../../contexts/LocalizationContext';
import { isTokenValid } from '../../utils/TokenUtils';
import DrawerNavigator from '../../navigators/DrawerNavigator';
import AuthNavigator from '../../navigators/AuthNavigator';
import { getEnvVars } from '../../../Environment';

const { localization } = getEnvVars();

i18n.defaultSeparator = '::';

const cloneT = i18n.t;
i18n.t = (key, ...args) => {
  if (key.slice(0, 2) === '::') {
    key = localization.defaultResourceName + key;
  }
  return cloneT(key, ...args);
};

function AppContainer({ language, fetchAppConfig, token, setToken }) {
  const platform = Platform.OS;
  const [isReady, setIsReady] = useState(false);

  const localizationContext = useMemo(
    () => ({
      t: i18n.t,
      locale: (language || {}).cultureName,
    }),
    [language],
  );

  const isValid = useMemo(() => isTokenValid(token), [token]);

  useEffect(() => {
    if (!isValid && token && token.access_token) {
      setToken({});
    }
  }, [isValid]);

  useEffect(() => {
    fetchAppConfig();

    Font.loadAsync({
      Roboto: require('native-base/Fonts/Roboto.ttf'),
      Roboto_medium: require('native-base/Fonts/Roboto_medium.ttf'),
      ...Ionicons.font,
    }).then(() => setIsReady(true));
  }, []);

  return (
    <>
      <StatusBar barStyle={platform === 'ios' ? 'dark-content' : 'light-content'} />
      <Root>
        {isReady && language ? (
          <LocalizationContext.Provider value={localizationContext}>
            <NavigationContainer>
              {isValid ? <DrawerNavigator /> : <AuthNavigator />}
            </NavigationContainer>
          </LocalizationContext.Provider>
        ) : null}
      </Root>
      <Loading />
    </>
  );
}

AppContainer.propTypes = {
  language: PropTypes.object,
  token: PropTypes.object.isRequired,
  fetchAppConfig: PropTypes.func.isRequired,
  setToken: PropTypes.func.isRequired,
};

export default connectToRedux({
  component: AppContainer,
  stateProps: state => ({
    language: createLanguageSelector()(state),
    token: createTokenSelector()(state),
  }),
  dispatchProps: {
    fetchAppConfig: AppActions.fetchAppConfigAsync,
    setToken: PersistentStorageActions.setToken,
  },
});
