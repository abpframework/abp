import { Injector } from '@angular/core';
import clone from 'just-clone';
import { of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { Environment } from '../models/environment';
import { AbpTenantService } from '../proxy/pages/abp/multi-tenancy/abp-tenant.service';
import { CurrentTenantDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { EnvironmentService } from '../services/environment.service';
import { MultiTenancyService } from '../services/multi-tenancy.service';
import { createTokenParser } from './string-utils';

const tenancyPlaceholder = '{0}';

function getCurrentTenancyName(appBaseUrl: string): string {
  if (appBaseUrl.charAt(appBaseUrl.length - 1) !== '/') appBaseUrl += '/';

  const parseTokens = createTokenParser(appBaseUrl);
  const token = tenancyPlaceholder.replace(/[}{]/g, '');
  return parseTokens(window.location.href)[token]?.[0];
}

export async function parseTenantFromUrl(injector: Injector) {
  const environmentService = injector.get(EnvironmentService);
  const multiTenancyService = injector.get(MultiTenancyService);
  const abpTenantService = injector.get(AbpTenantService);

  const baseUrl = environmentService.getEnvironment()?.application?.baseUrl || '';
  const tenancyName = getCurrentTenancyName(baseUrl);

  if (tenancyName) {
    multiTenancyService.isTenantBoxVisible = false;
    setEnvironment(injector, tenancyName);

    return of(null)
      .pipe(
        switchMap(() => abpTenantService.findTenantByName(tenancyName, { __tenant: '' })),
        tap(res => {
          multiTenancyService.domainTenant = res.success
            ? ({ id: res.tenantId, name: res.name } as CurrentTenantDto)
            : null;
        }),
      )
      .toPromise();
  }

  return Promise.resolve();
}

function setEnvironment(injector: Injector, tenancyName: string) {
  const environmentService = injector.get(EnvironmentService);

  const environment = clone(environmentService.getEnvironment()) as Environment;

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

  return environmentService.setState(environment);
}
