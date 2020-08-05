import { HttpClient } from '@angular/common/http';
import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { SetEnvironment } from '../actions/config.actions';
import { Config } from '../models/config';

export async function getRemoteEnv(injector: Injector, environment: Config.Environment) {
  const { remoteEnv } = environment;
  if (!remoteEnv?.url) return Promise.resolve();

  const http = injector.get(HttpClient);
  const store = injector.get(Store);

  const env = await http
    .request<Config.Environment>(remoteEnv.method || 'GET', remoteEnv.url, {
      headers: remoteEnv.headers || {},
    })
    .toPromise();

  return store.dispatch(new SetEnvironment({ ...environment, ...env })).toPromise();
}
