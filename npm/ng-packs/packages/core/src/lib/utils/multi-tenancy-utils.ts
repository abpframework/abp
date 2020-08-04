import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { switchMap, tap } from 'rxjs/operators';
import { SetEnvironment } from '../actions';
import { Config } from '../models/config';
import { MultiTenancyService } from '../services/multi-tenancy.service';
import { ConfigState } from '../states/config.state';
import { FormattedStringValueExtractor } from './formatted-string-value-extractor';

const tenancyPlaceholder = '{0}';

export function getCurrentTenancyNameOrNull(appBaseUrl: string): string {
  if (appBaseUrl.indexOf(tenancyPlaceholder) < 0) return null;

  if (appBaseUrl.charAt(appBaseUrl.length - 1) !== '/') appBaseUrl += '/';

  const currentRootAddress = window.location.href;

  const formattedStringValueExtractor = new FormattedStringValueExtractor();
  const values = formattedStringValueExtractor.isMatch(currentRootAddress, appBaseUrl);
  if (!values.length) {
    return null;
  }

  return values[0];
}

export async function parseTenantFromUrl(injector: Injector) {
  const store: Store = injector.get(Store);
  const multiTenancyService = injector.get(MultiTenancyService);
  const environment = store.selectSnapshot(ConfigState.getOne('environment')) as Config.Environment;

  const { baseUrl = '' } = environment.application;
  const tenancyName = getCurrentTenancyNameOrNull(baseUrl);

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

export function setEnvironment(store: Store, tenancyName: string) {
  const environment = store.selectSnapshot(ConfigState.getOne('environment')) as Config.Environment;

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
