import { createReducer } from '@reduxjs/toolkit';
import PersistentStorageActions from '../actions/PersistentStorageActions';

const initialState = { token: {}, language: null, tenant: {} };

export default createReducer(initialState, builder =>
  builder
    .addCase(PersistentStorageActions.setToken, (state, action) => {
      state.token = action.payload;
    })
    .addCase(PersistentStorageActions.setLanguage, (state, action) => {
      state.language = action.payload;
    })
    .addCase(PersistentStorageActions.setTenant, (state, action) => {
      state.tenant = action.payload;
    }),
);
