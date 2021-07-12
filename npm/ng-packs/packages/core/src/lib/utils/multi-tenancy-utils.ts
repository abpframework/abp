import { Injector } from '@angular/core';
import clone from 'just-clone';

import { tap } from 'rxjs/operators';
import { Environment } from '../models/environment';

import { FindTenantResultDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
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

function getCurrentTenancyNameFromUrl(tenantKey: string) {
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(tenantKey);
}

export async function parseTenantFromUrl(injector: Injector) {
  const environmentService = injector.get(EnvironmentService);
  const multiTenancyService = injector.get(MultiTenancyService);

  const baseUrl = environmentService.getEnvironment()?.application?.baseUrl || '';
  const tenancyName = getCurrentTenancyName(baseUrl);

  const hideTenantBox = () => {
    multiTenancyService.isTenantBoxVisible = false;
  };

  const setEnvironmentWithTenant = (tenant: FindTenantResultDto) => {
    hideTenantBox();
    replaceTenantNameWithinEnvironment(injector, tenant.name);
  };

  if (tenancyName) {
    return multiTenancyService
      .setTenantByName(tenancyName)
      .pipe(tap(setEnvironmentWithTenant))
      .toPromise();
  } else {
    /**
     * If there is no tenant, we still have to clean up {0}. from baseUrl to avoid incorrect http requests.
     */
    replaceTenantNameWithinEnvironment(injector, '', tenancyPlaceholder + '.');

    const tenantIdFromQueryParams = getCurrentTenancyNameFromUrl(multiTenancyService.tenantKey);
    if (tenantIdFromQueryParams) {
      return multiTenancyService.setTenantById(tenantIdFromQueryParams).toPromise();
    }
  }

  return Promise.resolve();
}

function replaceTenantNameWithinEnvironment(
  injector: Injector,
  tenancyName: string,
  placeholder = tenancyPlaceholder,
) {
  const environmentService = injector.get(EnvironmentService);

  const environment = clone(environmentService.getEnvironment()) as Environment;

  if (environment.application.baseUrl) {
    environment.application.baseUrl = environment.application.baseUrl.replace(
      placeholder,
      tenancyName,
    );
  }

  if (environment.oAuthConfig.redirectUri) {
    environment.oAuthConfig.redirectUri = environment.oAuthConfig.redirectUri.replace(
      placeholder,
      tenancyName,
    );
  }

  environment.oAuthConfig.issuer = environment.oAuthConfig.issuer.replace(placeholder, tenancyName);

  Object.keys(environment.apis).forEach(api => {
    Object.keys(environment.apis[api]).forEach(key => {
      environment.apis[api][key] = environment.apis[api][key].replace(placeholder, tenancyName);
    });
  });

  return environmentService.setState(environment);
}
