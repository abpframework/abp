import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';

@Injectable({
  providedIn: 'root',
})
export class ConfigService {
  constructor(private store: Store) {}

  getAll() {
    return this.store.selectSnapshot(ConfigState.getAll);
  }

  getOne(key: string) {
    return this.store.selectSnapshot(ConfigState.getOne(key));
  }

  getDeep(keys: string[] | string) {
    return this.store.selectSnapshot(ConfigState.getDeep(keys));
  }

  getSetting(key: string) {
    return this.store.selectSnapshot(ConfigState.getSetting(key));
  }
}
