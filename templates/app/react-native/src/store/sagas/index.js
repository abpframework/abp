import { all, fork } from 'redux-saga/effects';
import AppSaga from './AppSaga';

export function* rootSaga() {
  yield all([fork(AppSaga)]);
}
