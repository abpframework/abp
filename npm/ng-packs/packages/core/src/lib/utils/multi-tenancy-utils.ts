import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states/config.state';
import { Config } from '../models/config';
import { FormattedStringValueExtractor } from './formatted-string-value-extractor';
import { MultiTenancyService } from '../services/multi-tenancy.service';
import { tap, switchMap } from 'rxjs/operators';
import { SetTenant, SetEnvironment } from '../actions';
import { of } from 'rxjs';

const tenancyPlaceholder = '{TENANCY_NAME}';

export function getCurrentTenancyNameOrNull(appBaseUrl: string): string {
  if (appBaseUrl.indexOf(tenancyPlaceholder) < 0) return null;

  const currentRootAddress = document.location.href;

  const formattedStringValueExtracter = new FormattedStringValueExtractor();
  const values: any[] = formattedStringValueExtracter.isMatch(currentRootAddress, appBaseUrl);
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
          if (!res.success) return;
          const tenant = { id: res.tenantId, name: res.name };
          multiTenancyService.domainTenant = tenant;
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
