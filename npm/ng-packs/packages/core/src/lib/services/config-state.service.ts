import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
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

  getOne(...args: Parameters<typeof ConfigState.getOne>) {
    return this.store.selectSnapshot(ConfigState.getOne(...args));
  }

  getDeep(...args: Parameters<typeof ConfigState.getDeep>) {
    return this.store.selectSnapshot(ConfigState.getDeep(...args));
  }

  getRoute(...args: Parameters<typeof ConfigState.getRoute>) {
    return this.store.selectSnapshot(ConfigState.getRoute(...args));
  }

  getApiUrl(...args: Parameters<typeof ConfigState.getApiUrl>) {
    return this.store.selectSnapshot(ConfigState.getApiUrl(...args));
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
}
