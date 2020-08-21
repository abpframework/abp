import { HttpClient } from '@angular/common/http';
import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { catchError, switchMap } from 'rxjs/operators';
import { SetEnvironment } from '../actions/config.actions';
import { Config } from '../models/config';
import { RestOccurError } from '../actions/rest.actions';
import { deepMerge } from './object-utils';

export function getRemoteEnv(injector: Injector, environment: Partial<Config.Environment>) {
  const { remoteEnv } = environment;
  const { headers = {}, method = 'GET', url } = remoteEnv || ({} as Config.RemoteEnv);
  if (!url) return Promise.resolve();

  const http = injector.get(HttpClient);
  const store = injector.get(Store);

  return http
    .request<Config.Environment>(method, url, { headers })
    .pipe(
      catchError(err => store.dispatch(new RestOccurError(err))), // TODO: Condiser get handle function from a provider
      switchMap(env => store.dispatch(mergeEnvironments(environment, env, remoteEnv))),
    )
    .toPromise();
}

function mergeEnvironments(
  local: Partial<Config.Environment>,
  remote: any,
  config: Config.RemoteEnv,
) {
  switch (config.mergeStrategy) {
    case 'deepmerge':
      return new SetEnvironment(deepMerge(local, remote));
    case 'overwrite':
    case null:
    case undefined:
      return new SetEnvironment(remote);
    default:
      return new SetEnvironment(config.mergeStrategy(local, remote));
  }
}
