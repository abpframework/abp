import { Injectable, inject, PLATFORM_ID, RESPONSE_INIT } from '@angular/core';
import {
  UrlTree,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateFn, Params,
} from '@angular/router';

import { Observable, timer, filter, take, map, firstValueFrom, timeout, catchError, of } from 'rxjs';
import { OAuthService } from 'angular-oauth2-oidc';

import { AuthService, IAbpGuard, EnvironmentService } from '@abp/ng.core';
import { isPlatformServer } from '@angular/common';

/**
 * @deprecated Use `abpOAuthGuard` *function* instead.
 */
@Injectable({
  providedIn: 'root',
})
export class AbpOAuthGuard implements IAbpGuard {
  protected readonly oAuthService = inject(OAuthService);
  protected readonly authService = inject(AuthService);

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean> | boolean | UrlTree {
    const hasValidAccessToken = this.oAuthService.hasValidAccessToken();
    if (hasValidAccessToken) {
      return true;
    }

    const params = { returnUrl: state.url };
    this.authService.navigateToLogin(params);
    return false;
  }
}

export const abpOAuthGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const oAuthService = inject(OAuthService);
  const authService = inject(AuthService);
  const platformId = inject(PLATFORM_ID);
  const resInit = inject(RESPONSE_INIT);
  const environmentService = inject(EnvironmentService);

  const hasValidAccessToken = oAuthService.hasValidAccessToken();

  if (hasValidAccessToken) {
    return true;
  }

  const params = { returnUrl: state.url };
  if (isPlatformServer(platformId) && resInit) {
    const ssrAuthorizationUrl = environmentService.getEnvironment().oAuthConfig.ssrAuthorizationUrl;
    const url = buildLoginUrl(ssrAuthorizationUrl, params);
    const headers = new Headers(resInit.headers);
    headers.set('Location', url);
    resInit.status = 302;
    resInit.statusText = 'Found';
    resInit.headers = headers;
    return;
  }
  authService.navigateToLogin(params);
  return false;
};

export const buildLoginUrl = (path: string, params?: Params): string => {
  if (!params || Object.keys(params).length === 0) return path;
  const usp = new URLSearchParams();
  for (const [k, v] of Object.entries(params)) {
    if (v == null) continue;
    Array.isArray(v) ? v.forEach(x => usp.append(k, String(x))) : usp.set(k, String(v));
  }
  return `${path}?${usp.toString()}`;
}

export const asyncAbpOAuthGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const oAuthService = inject(OAuthService);
  const authService = inject(AuthService);
  const environmentService = inject(EnvironmentService);
  const platformId = inject(PLATFORM_ID);
  const resInit = inject(RESPONSE_INIT);

  const { oAuthConfig } = environmentService.getEnvironment();

  if (oAuthConfig?.responseType === 'code') {
    return firstValueFrom(
      timer(0, 100).pipe(
        map(() => oAuthService.hasValidAccessToken()),
        filter(Boolean),
        take(1),
        timeout(3000),
        catchError(() => {
          if (isPlatformServer(platformId) && resInit) {
            const ssrAuthorizationUrl = environmentService.getEnvironment().oAuthConfig.ssrAuthorizationUrl;
            const url = buildLoginUrl(ssrAuthorizationUrl, { returnUrl: state.url });
            const headers = new Headers(resInit.headers);
            headers.set('Location', url);
            resInit.status = 302;
            resInit.statusText = 'Found';
            resInit.headers = headers;
            return;
          }
          authService.navigateToLogin({ returnUrl: state.url });
          return of(false);
        })
      )
    );
  }

  if (oAuthService.hasValidAccessToken()) {
    return true;
  }

  authService.navigateToLogin({ returnUrl: state.url });
  return false;
};
