import { all, call, put, takeLatest } from 'redux-saga/effects';
import { Logout } from '../../api/AccountAPI';
import { getApplicationConfiguration } from '../../api/ApplicationConfigurationAPI';
import AppActions from '../actions/AppActions';
import LoadingActions from '../actions/LoadingActions';
import PersistentStorageActions from '../actions/PersistentStorageActions';

function* fetchAppConfig({ payload: { showLoading, callback } }) {
  if (showLoading) yield put(LoadingActions.start({ key: 'appConfig', opacity: 1 }));
  const data = yield call(getApplicationConfiguration);
  yield put(AppActions.setAppConfig(data));
  yield put(PersistentStorageActions.setLanguage(data.localization.currentCulture.cultureName));
  if (showLoading) yield put(LoadingActions.stop({ key: 'appConfig' }));
  callback();
}

function* setLanguage(action) {
  yield put(PersistentStorageActions.setLanguage(action.payload));
  yield put(AppActions.fetchAppConfigAsync());
}

function* logout() {
  yield call(Logout);
  yield put(PersistentStorageActions.setToken({}));
  yield put(AppActions.fetchAppConfigAsync());
}

export default function*() {
  yield all([
    takeLatest(AppActions.setLanguageAsync.type, setLanguage),
    takeLatest(AppActions.fetchAppConfigAsync.type, fetchAppConfig),
    takeLatest(AppActions.logoutAsync.type, logout),
  ]);
}
