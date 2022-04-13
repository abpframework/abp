import { combineReducers } from '@reduxjs/toolkit';
import AppReducer from './AppReducer';
import LoadingReducer from './LoadingReducer';
import PersistentStorageReducer from './PersistentStorageReducer';

const rootReducer = combineReducers({
  loading: LoadingReducer,
  app: AppReducer,
  persistentStorage: PersistentStorageReducer,
});

export default rootReducer;
