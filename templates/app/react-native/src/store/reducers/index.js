import { combineReducers } from '@reduxjs/toolkit';
import LoadingReducer from './LoadingReducer';
import AppReducer from './AppReducer';
import PersistentStorageReducer from './PersistentStorageReducer';

const rootReducer = combineReducers({
  loading: LoadingReducer,
  app: AppReducer,
  persistentStorage: PersistentStorageReducer,
});

export default rootReducer;
