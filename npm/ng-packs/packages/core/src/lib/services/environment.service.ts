import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Apis, Environment } from '../models/environment';
import { InternalStore } from '../utils/internal-store-utils';

const mapToApiUrl = (key: string | undefined) => (apis: Apis) =>
  ((key && apis[key]) || apis.default).url || apis.default.url;

const mapToIssuer = (issuer: string | undefined) => {
  if (!issuer) {
    return issuer;
  }
  return issuer.endsWith('/') ? issuer : issuer + '/';
};

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

  getApiUrl(key: string | undefined) {
    return mapToApiUrl(key)(this.store.state?.apis);
  }

  getApiUrl$(key: string) {
    return this.store.sliceState(state => state.apis).pipe(map(mapToApiUrl(key)));
  }

  setState(environment: Environment) {
    this.store.set(environment);
  }

  getIssuer() {
    const issuer = this.store.state?.oAuthConfig?.issuer;

    return mapToIssuer(issuer);
  }

  getIssuer$() {
    return this.store.sliceState(state => state?.oAuthConfig?.issuer).pipe(map(mapToIssuer));
  }

  getImpersonation() {
    return this.store.state?.oAuthConfig?.impersonation || {};
  }

  getImpersonation$() {
    return this.store.sliceState(state => state?.oAuthConfig?.impersonation || {});
  }
}
