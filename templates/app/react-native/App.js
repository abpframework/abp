import { StyleProvider } from 'native-base';
import React from 'react';
import { enableScreens } from 'react-native-screens';
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import AppContainer from './src/components/AppContainer/AppContainer';
import { store, persistor } from './src/store';
import getTheme from './src/theme/components';
import { activeTheme } from './src/theme/variables';
import { initAPIInterceptor } from './src/interceptors/APIInterceptor';

enableScreens();
initAPIInterceptor(store);

export default function App() {
  return (
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <StyleProvider style={getTheme(activeTheme)}>
          <AppContainer />
        </StyleProvider>
      </PersistGate>
    </Provider>
  );
}
