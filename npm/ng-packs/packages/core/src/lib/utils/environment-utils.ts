import { HttpClient } from '@angular/common/http';
import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { catchError, tap } from 'rxjs/operators';
import { RestOccurError } from '../actions/rest.actions';
import { Environment, RemoteEnv } from '../models/environment';
import { EnvironmentService } from '../services/environment.service';
import { deepMerge } from './object-utils';

export function getRemoteEnv(injector: Injector, environment: Partial<Environment>) {
  const environmentService = injector.get(EnvironmentService);

  const { remoteEnv } = environment;
  const { headers = {}, method = 'GET', url } = remoteEnv || ({} as RemoteEnv);
  if (!url) return Promise.resolve();

  const http = injector.get(HttpClient);
  const store = injector.get(Store);

  return http
    .request<Environment>(method, url, { headers })
    .pipe(
      catchError(err => store.dispatch(new RestOccurError(err))), // TODO: Condiser get handle function from a provider
      tap(env => environmentService.setState(mergeEnvironments(environment, env, remoteEnv))),
    )
    .toPromise();
}

function mergeEnvironments(
  local: Partial<Environment>,
  remote: any,
  config: RemoteEnv,
): Environment {
  switch (config.mergeStrategy) {
    case 'deepmerge':
      return deepMerge(local, remote);
    case 'overwrite':
    case null:
    case undefined:
      return remote;
    default:
      return config.mergeStrategy(local, remote);
  }
}
