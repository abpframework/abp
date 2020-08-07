import { HttpClient } from '@angular/common/http';
import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { catchError, switchMap } from 'rxjs/operators';
import { SetEnvironment } from '../actions/config.actions';
import { Config } from '../models/config';
import { RestOccurError } from '../actions/rest.actions';

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
      switchMap(env => store.dispatch(new SetEnvironment({ ...environment, ...env }))),
    )
    .toPromise();
}
