import { HttpClient } from '@angular/common/http';
import { Injector, isDevMode } from '@angular/core';
import { of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Environment, RemoteEnv } from '../models/environment';
import { EnvironmentService } from '../services/environment.service';
import { deepMerge } from './object-utils';

export function getRemoteEnv(injector: Injector, environment: Partial<Environment>) {
  const environmentService = injector.get(EnvironmentService);

  const { remoteEnv } = environment;
  const { headers = {}, method = 'GET', url } = remoteEnv || ({} as RemoteEnv);
  if (!url) return Promise.resolve();

  const http = injector.get(HttpClient);

  return http
    .request<Environment>(method, url, { headers })
    .pipe(
      catchError(err => {
        if (isDevMode()) {
          console.warn(
            `[ABP Environment] Failed to fetch remote environment from "${url}". ` +
              `Error: ${err.message || err}\n` +
              `See https://abp.io/docs/latest/framework/ui/angular/environment#example-remoteenv-configuration for configuration details.`,
          );
        }
        return of(null);
      }),
      tap(env =>
        environmentService.setState(
          mergeEnvironments(environment, env || ({} as Environment), remoteEnv as RemoteEnv),
        ),
      ),
    )
    .toPromise();
}

function mergeEnvironments(
  local: Partial<Environment>,
  remote: Environment,
  config: RemoteEnv,
): Environment {
  switch (config.mergeStrategy) {
    case 'deepmerge':
      return deepMerge(local, remote) as Environment;
    case 'overwrite':
    case null:
    case undefined:
      return remote;
    default:
      return config.mergeStrategy(local, remote);
  }
}
