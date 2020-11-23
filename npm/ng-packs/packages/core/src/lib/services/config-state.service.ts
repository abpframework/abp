import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApplicationConfiguration } from '../models/application-configuration';
import { InternalStore } from '../utils/internal-store-utils';

@Injectable({
  providedIn: 'root',
})
export class ConfigStateService {
  private readonly store = new InternalStore({} as ApplicationConfiguration.Response);

  get createOnUpdateStream() {
    return this.store.sliceUpdate;
  }

  setState = (state: ApplicationConfiguration.Response) => {
    this.store.set(state);
  };

  getOne$(key: string) {
    return this.store.sliceState(state => state[key]);
  }

  getOne(key: string) {
    return this.store.state[key];
  }

  getAll$(): Observable<ApplicationConfiguration.Response> {
    return this.store.sliceState(state => state);
  }

  getAll(): ApplicationConfiguration.Response {
    return this.store.state;
  }

  getDeep$(keys: string[] | string) {
    keys = splitKeys(keys);

    return this.store
      .sliceState(state => state)
      .pipe(
        map(state => {
          return (keys as string[]).reduce((acc, val) => {
            if (acc) {
              return acc[val];
            }

            return undefined;
          }, state);
        }),
      );
  }

  getDeep(keys: string[] | string) {
    keys = splitKeys(keys);

    return (keys as string[]).reduce((acc, val) => {
      if (acc) {
        return acc[val];
      }

      return undefined;
    }, this.store.state);
  }

  getFeature(key: string) {
    return this.store.state.features?.values?.[key];
  }

  getFeature$(key: string) {
    return this.store.sliceState(state => state.features?.values?.[key]);
  }

  getSetting(key: string) {
    return this.store.state.setting?.values?.[key];
  }

  getSetting$(key: string) {
    return this.store.sliceState(state => state.setting?.values?.[key]);
  }

  getSettings(keyword?: string) {
    const settings = this.store.state.setting?.values || {};

    if (!keyword) return settings;

    const keysFound = Object.keys(settings).filter(key => key.indexOf(keyword) > -1);

    return keysFound.reduce((acc, key) => {
      acc[key] = settings[key];
      return acc;
    }, {});
  }

  getSettings$(keyword?: string) {
    return this.store
      .sliceState(state => state.setting?.values)
      .pipe(
        map((settings = {}) => {
          if (!keyword) return settings;

          const keysFound = Object.keys(settings).filter(key => key.indexOf(keyword) > -1);

          return keysFound.reduce((acc, key) => {
            acc[key] = settings[key];
            return acc;
          }, {});
        }),
      );
  }
}

function splitKeys(keys: string[] | string): string[] {
  if (typeof keys === 'string') {
    keys = keys.split('.');
  }

  if (!Array.isArray(keys)) {
    throw new Error('The argument must be a dot string or an string array.');
  }

  return keys;
}
