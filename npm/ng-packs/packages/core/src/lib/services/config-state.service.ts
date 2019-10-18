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

  getOne(...args: Parameters<typeof ConfigState.getOne>) {
    return this.store.selectSnapshot(ConfigState.getOne(...args));
  }

  getDeep(...args: Parameters<typeof ConfigState.getDeep>) {
    return this.store.selectSnapshot(ConfigState.getDeep(...args));
  }

  getSetting(...args: Parameters<typeof ConfigState.getSetting>) {
    return this.store.selectSnapshot(ConfigState.getSetting(...args));
  }
}
