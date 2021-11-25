import { NavigationContainer } from '@react-navigation/native';
import i18n from 'i18n-js';
import { Root } from 'native-base';
import PropTypes from 'prop-types';
import React, { useEffect, useMemo } from 'react';
import { Platform, StatusBar } from 'react-native';
import { getEnvVars } from '../../../Environment';
import { LocalizationContext } from '../../contexts/LocalizationContext';
import AuthNavigator from '../../navigators/AuthNavigator';
import DrawerNavigator from '../../navigators/DrawerNavigator';
import AppActions from '../../store/actions/AppActions';
import PersistentStorageActions from '../../store/actions/PersistentStorageActions';
import { createLanguageSelector } from '../../store/selectors/AppSelectors';
import { createTokenSelector } from '../../store/selectors/PersistentStorageSelectors';
import { connectToRedux } from '../../utils/ReduxConnect';
import { isTokenValid } from '../../utils/TokenUtils';
import Loading from '../Loading/Loading';

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
  }, []);

  return (
    <>
      <StatusBar barStyle={platform === 'ios' ? 'dark-content' : 'light-content'} />
      <Root>
        {language ? (
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
