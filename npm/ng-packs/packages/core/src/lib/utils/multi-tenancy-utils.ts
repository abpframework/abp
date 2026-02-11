import { Injector, PLATFORM_ID } from '@angular/core';
import clone from 'just-clone';
import { Environment } from '../models/environment';
import { DOCUMENT, isPlatformBrowser } from '@angular/common';

import { FindTenantResultDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { EnvironmentService } from '../services/environment.service';
import { MultiTenancyService } from '../services/multi-tenancy.service';
import { createTokenParser } from './string-utils';
import { firstValueFrom } from 'rxjs';
import { TENANT_NOT_FOUND_BY_NAME } from '../tokens/tenant-not-found-by-name';
import { HttpErrorResponse } from '@angular/common/http';

const tenancyPlaceholder = '{0}';

function getCurrentTenancyName(appBaseUrl: string, injector: Injector): string {
  const platformId = injector.get(PLATFORM_ID);
  const document = injector.get(DOCUMENT);
  if (appBaseUrl.charAt(appBaseUrl.length - 1) !== '/') appBaseUrl += '/';

  const parseTokens = createTokenParser(appBaseUrl);
  const token = tenancyPlaceholder.replace(/[}{]/g, '');
  const tokenValue = isPlatformBrowser(platformId)
    ? parseTokens(document.defaultView?.location.href)[token]?.[0]
    : undefined;
  return tokenValue;
}

export function getCurrentTenancyNameFromUrl(tenantKey: string, injector): string | null {
  const platformId = injector.get(PLATFORM_ID);
  const document = injector.get(DOCUMENT);
  if (!isPlatformBrowser(platformId)) {
    return null;
  }
  const search = document.defaultView?.location.search;
  const urlParams = new URLSearchParams(search);
  return urlParams.get(tenantKey);
}

export async function parseTenantFromUrl(injector: Injector) {
  const environmentService = injector.get(EnvironmentService);
  const multiTenancyService = injector.get(MultiTenancyService);
  const tenantNotFoundHandler = injector.get(TENANT_NOT_FOUND_BY_NAME, null);

  const baseUrl = environmentService.getEnvironment()?.application?.baseUrl || '';
  const tenancyName = getCurrentTenancyName(baseUrl, injector);

  const hideTenantBox = () => {
    multiTenancyService.isTenantBoxVisible = false;
  };

  const setDomainTenant = (tenant: FindTenantResultDto) => {
    multiTenancyService.domainTenant = {
      id: tenant.tenantId,
      name: tenant.name,
      isAvailable: true,
    };
  };

  const setEnvironmentWithDomainTenant = (tenant: FindTenantResultDto) => {
    hideTenantBox();
    setDomainTenant(tenant);
  };

  if (tenancyName) {
    /**
     * We have to replace tenant name within the urls from environment,
     * because the code below will make a http request to find information about the domain tenant.
     * Before this request takes place, we need to replace placeholders aka "{0}".
     */
    replaceTenantNameWithinEnvironment(injector, tenancyName);

    const tenant$ = multiTenancyService.setTenantByName(tenancyName);
    try {
      const result = await firstValueFrom(tenant$);
      setEnvironmentWithDomainTenant(result);
      return Promise.resolve(result);
    } catch (httpError: HttpErrorResponse | any) {
      if (
        httpError instanceof HttpErrorResponse &&
        httpError.status === 404 &&
        tenantNotFoundHandler
      ) {
        tenantNotFoundHandler(httpError);
      }
      return Promise.reject();
    }
  }
  /**
   * If there is no tenant, we still have to clean up {0}. from baseUrl to avoid incorrect http requests.
   */
  replaceTenantNameWithinEnvironment(injector, '', tenancyPlaceholder + '.');

  const tenantIdFromQueryParams = getCurrentTenancyNameFromUrl(
    multiTenancyService.tenantKey,
    injector,
  );
  if (tenantIdFromQueryParams) {
    const tenantById$ = multiTenancyService.setTenantById(tenantIdFromQueryParams);
    return firstValueFrom(tenantById$);
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

  if (environment.oAuthConfig?.redirectUri) {
    environment.oAuthConfig.redirectUri = environment.oAuthConfig.redirectUri.replace(
      placeholder,
      tenancyName,
    );
  }

  if (!environment.oAuthConfig) {
    environment.oAuthConfig = {};
  }
  environment.oAuthConfig.issuer = (environment.oAuthConfig.issuer || '').replace(
    placeholder,
    tenancyName,
  );

  Object.keys(environment.apis).forEach(api => {
    Object.keys(environment.apis[api]).forEach(key => {
      environment.apis[api][key] = (environment.apis[api][key] || '').replace(
        placeholder,
        tenancyName,
      );
    });
  });

  return environmentService.setState(environment);
}
