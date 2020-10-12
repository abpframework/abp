import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { GetAppConfiguration, SetEnvironment } from '../actions/config.actions';
import { ConfigState } from '../states';

@Injectable({
  providedIn: 'root',
})
export class ConfigStateService {
  constructor(private store: Store) {}

  getAll() {
    return this.store.selectSnapshot(ConfigState.getAll);
  }

  getApplicationInfo() {
    return this.store.selectSnapshot(ConfigState.getApplicationInfo);
  }

  getEnvironment() {
    return this.store.selectSnapshot(ConfigState.getEnvironment);
  }

  getOne(...args: Parameters<typeof ConfigState.getOne>) {
    return this.store.selectSnapshot(ConfigState.getOne(...args));
  }

  getDeep(...args: Parameters<typeof ConfigState.getDeep>) {
    return this.store.selectSnapshot(ConfigState.getDeep(...args));
  }

  getApiUrl(...args: Parameters<typeof ConfigState.getApiUrl>) {
    return this.store.selectSnapshot(ConfigState.getApiUrl(...args));
  }

  getFeature(...args: Parameters<typeof ConfigState.getFeature>) {
    return this.store.selectSnapshot(ConfigState.getFeature(...args));
  }

  getSetting(...args: Parameters<typeof ConfigState.getSetting>) {
    return this.store.selectSnapshot(ConfigState.getSetting(...args));
  }

  getSettings(...args: Parameters<typeof ConfigState.getSettings>) {
    return this.store.selectSnapshot(ConfigState.getSettings(...args));
  }

  getGrantedPolicy(...args: Parameters<typeof ConfigState.getGrantedPolicy>) {
    return this.store.selectSnapshot(ConfigState.getGrantedPolicy(...args));
  }

  getLocalization(...args: Parameters<typeof ConfigState.getLocalization>) {
    return this.store.selectSnapshot(ConfigState.getLocalization(...args));
  }

  getLocalizationResource(...args: Parameters<typeof ConfigState.getLocalizationResource>) {
    return this.store.selectSnapshot(ConfigState.getLocalizationResource(...args));
  }

  dispatchGetAppConfiguration() {
    return this.store.dispatch(new GetAppConfiguration());
  }

  dispatchSetEnvironment(...args: ConstructorParameters<typeof SetEnvironment>) {
    return this.store.dispatch(new SetEnvironment(...args));
  }
}
