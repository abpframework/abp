import { createReducer } from '@reduxjs/toolkit';
import AppActions from '../actions/AppActions';

const initialState = {
  appConfig: {},
};

export default createReducer(initialState, builder =>
  builder.addCase(AppActions.setAppConfig, (state, action) => {
    state.appConfig = action.payload;
  }),
);
