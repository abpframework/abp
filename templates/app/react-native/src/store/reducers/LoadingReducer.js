import { createReducer } from '@reduxjs/toolkit';
import LoadingActions from '../actions/LoadingActions';

const initialState = { activeLoadings: {}, loading: false };

export default createReducer(initialState, builder =>
  builder
    .addCase(LoadingActions.start, (state, action) => {
      const { key, opacity } = action.payload;
      return {
        ...state,
        actives: { ...state.activeLoadings, [key]: action },
        loading: true,
        opacity,
      };
    })
    .addCase(LoadingActions.stop, (state, action) => {
      delete state.activeLoadings[action.payload.key];

      state.loading = !!Object.keys(state.activeLoadings).length;
    })
    .addCase(LoadingActions.clear, () => ({})),
);
