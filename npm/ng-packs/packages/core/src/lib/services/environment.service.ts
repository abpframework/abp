import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Apis, Environment } from '../models/environment';
import { InternalStore } from '../utils/internal-store-utils';

@Injectable({ providedIn: 'root' })
export class EnvironmentService {
  private readonly store = new InternalStore({} as Environment);

  get createOnUpdateStream() {
    return this.store.sliceUpdate;
  }

  getEnvironment$(): Observable<Environment> {
    return this.store.sliceState(state => state);
  }

  getEnvironment(): Environment {
    return this.store.state;
  }

  getApiUrl(key?: string) {
    return (this.store.state.apis[key || 'default'] || this.store.state.apis.default).url;
  }

  getApiUrl$(key?: string) {
    return this.store
      .sliceState(state => state.apis)
      .pipe(map((apis: Apis) => (apis[key || 'default'] || apis.default).url));
  }

  setState(environment: Environment) {
    this.store.set(environment);
  }
}
