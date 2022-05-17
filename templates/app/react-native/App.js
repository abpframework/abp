import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import i18n from 'i18n-js';
import { NativeBaseProvider } from 'native-base';
import React, { useEffect, useMemo, useState } from 'react';
import { enableScreens } from 'react-native-screens';
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import { getEnvVars } from './Environment';
import Loading from './src/components/Loading/Loading';
import { LocalizationContext } from './src/contexts/LocalizationContext';
import { initAPIInterceptor } from './src/interceptors/APIInterceptor';
import AuthNavigator from './src/navigators/AuthNavigator';
import DrawerNavigator from './src/navigators/DrawerNavigator';
import { persistor, store } from './src/store';
import AppActions from './src/store/actions/AppActions';
import PersistentStorageActions from './src/store/actions/PersistentStorageActions';
import { createLanguageSelector } from './src/store/selectors/AppSelectors';
import { createTokenSelector } from './src/store/selectors/PersistentStorageSelectors';
import { connectToRedux } from './src/utils/ReduxConnect';
import { isTokenValid } from './src/utils/TokenUtils';

const Stack = createNativeStackNavigator();

const { localization } = getEnvVars();

i18n.defaultSeparator = '::';

const cloneT = i18n.t;
i18n.t = (key, ...args) => {
  if (key.slice(0, 2) === '::') {
    key = localization.defaultResourceName + key;
  }
  return cloneT(key, ...args);
};

enableScreens();
initAPIInterceptor(store);

export default function App() {
  const language = createLanguageSelector()(store.getState());
  const [isReady, setIsReady] = useState(false);

  const localizationContextValue = useMemo(
    () => ({
      t: i18n.t,
      locale: (language || {}).cultureName,
    }),
    [language]
  );

  useEffect(() => {
    store.dispatch(
      AppActions.fetchAppConfigAsync({
        callback: () => setIsReady(true),
        showLoading: true,
      })
    );
  }, []);

  return (
    <NavigationContainer>
      <Provider store={store}>
        <PersistGate loading={null} persistor={persistor}>
          <NativeBaseProvider>
            {isReady ? (
              <LocalizationContext.Provider value={localizationContextValue}>
                <ConnectedAppContainer />
              </LocalizationContext.Provider>
            ) : null}
            <Loading />
          </NativeBaseProvider>
        </PersistGate>
      </Provider>
    </NavigationContainer>
  );
}

function AppContainer({token, setToken}) {
  const isValid = useMemo(() => isTokenValid(token), [token]);

  useEffect(() => {
    if (!isValid && token && token.access_token) {
      setToken({})
    }
  }, [isValid]);


  return isValid ? <DrawerNavigator /> : <AuthNavigator />
}


const ConnectedAppContainer = connectToRedux({
  component: AppContainer,
  stateProps: (state) => ({
    token: createTokenSelector()(state),
  }),
  dispatchProps: {
    setToken: PersistentStorageActions.setToken,
  },
});
