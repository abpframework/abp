import { HttpClient } from '@angular/common/http';
import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { SetEnvironment } from '../actions/config.actions';
import { Config } from '../models/config';

export function getRemoteEnv(injector: Injector, environment: Partial<Config.Environment>) {
  const { remoteEnv } = environment;
  const { headers = {}, method = 'GET', url } = remoteEnv || ({} as Config.RemoteEnv);
  if (!url) return Promise.resolve();

  const http = injector.get(HttpClient);
  const store = injector.get(Store);

  return http
    .request<Config.Environment>(method, url, { headers })
    .pipe(
      catchError(() => of({} as Config.Environment)), // TODO: Handle error
      switchMap(env => store.dispatch(new SetEnvironment({ ...environment, ...env }))),
    )
    .toPromise();
}
