import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import clone from 'just-clone';
import { switchMap, tap } from 'rxjs/operators';
import { SetEnvironment } from '../actions';
import { Config } from '../models/config';
import { MultiTenancyService } from '../services/multi-tenancy.service';
import { ConfigState } from '../states/config.state';
import { createTokenParser } from './string-utils';

const tenancyPlaceholder = '{0}';

function getCurrentTenancyName(appBaseUrl: string): string {
  if (appBaseUrl.charAt(appBaseUrl.length - 1) !== '/') appBaseUrl += '/';

  const parseTokens = createTokenParser(appBaseUrl);
  const token = tenancyPlaceholder.replace(/[}{]/g, '');
  return parseTokens(window.location.href)[token]?.[0];
}

export async function parseTenantFromUrl(injector: Injector) {
  const store: Store = injector.get(Store);
  const multiTenancyService = injector.get(MultiTenancyService);
  const environment = store.selectSnapshot(ConfigState.getOne('environment')) as Config.Environment;

  const { baseUrl = '' } = environment.application;
  const tenancyName = getCurrentTenancyName(baseUrl);

  if (tenancyName) {
    multiTenancyService.isTenantBoxVisible = false;

    return setEnvironment(store, tenancyName)
      .pipe(
        switchMap(() => multiTenancyService.findTenantByName(tenancyName, { __tenant: '' })),
        tap(res => {
          multiTenancyService.domainTenant = res.success
            ? { id: res.tenantId, name: res.name }
            : null;
        }),
      )
      .toPromise();
  }

  return Promise.resolve();
}

function setEnvironment(store: Store, tenancyName: string) {
  const environment = clone(
    store.selectSnapshot(ConfigState.getOne('environment')),
  ) as Config.Environment;

  if (environment.application.baseUrl) {
    environment.application.baseUrl = environment.application.baseUrl.replace(
      tenancyPlaceholder,
      tenancyName,
    );
  }

  environment.oAuthConfig.issuer = environment.oAuthConfig.issuer.replace(
    tenancyPlaceholder,
    tenancyName,
  );

  Object.keys(environment.apis).forEach(api => {
    Object.keys(environment.apis[api]).forEach(key => {
      environment.apis[api][key] = environment.apis[api][key].replace(
        tenancyPlaceholder,
        tenancyName,
      );
    });
  });

  return store.dispatch(new SetEnvironment(environment));
}
