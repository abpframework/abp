import { createSelector } from 'reselect';

const getLoading = state => state.loading;

export function createLoadingSelector() {
  return createSelector([getLoading], loading => loading.loading);
}

export function createOpacitySelector() {
  return createSelector([getLoading], loading => loading.opacity);
}
