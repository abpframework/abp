import { Injector } from '@angular/core';
import { Location } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Params, Router } from '@angular/router';

import { Observable, of } from 'rxjs';
import { filter, map, switchMap, take, tap } from 'rxjs/operators';
import {
  AuthConfig,
  OAuthErrorEvent,
  OAuthService as OAuthService2,
  OAuthStorage,
} from 'angular-oauth2-oidc';

import {
  AbpLocalStorageService,
  AbpWindowService,
  AuthService,
  ConfigStateService,
  EnvironmentService,
  HttpErrorReporterService,
  LoginParams,
  SessionStateService,
  TENANT_KEY,
} from '@abp/ng.core';

import { clearOAuthStorage } from '../utils/clear-o-auth-storage';
import { OAuthErrorFilterService } from '../services';
import { isTokenExpired, isTransientTokenError } from '../utils';
import { RememberMeService } from '../services/remember-me.service';
import { MemoryTokenStorageService } from '../services/memory-token-storage.service';

export abstract class AuthFlowStrategy {
  abstract readonly isInternalAuth: boolean;

  /**
   * Starts the sign-in. Returns a promise only for a redirect that leaves this page. It resolves
   * when the page is still here after the redirect, and rejects when the login page can't be opened.
   */
  startLogin(queryParams?: Params): Promise<void> | undefined {
    this.navigateToLogin(queryParams);
    return undefined;
  }

  /** Whether startLogin() opens the login page with the default full-page redirect. */
  usesDefaultRedirect(): boolean {
    return false;
  }

  protected httpErrorReporter: HttpErrorReporterService;
  protected environment: EnvironmentService;
  protected configState: ConfigStateService;
  protected oAuthService: OAuthService2;
  protected oAuthConfig!: AuthConfig;
  protected sessionState: SessionStateService;
  protected localStorageService: AbpLocalStorageService;
  protected rememberMeService: RememberMeService;
  protected windowService: AbpWindowService;
  protected tenantKey: string;
  protected router: Router;

  protected readonly oAuthErrorFilterService: OAuthErrorFilterService;

  abstract checkIfInternalAuth(queryParams?: Params): boolean;
  abstract navigateToLogin(queryParams?: Params): void;
  abstract logout(queryParams?: Params): Observable<any>;
  abstract login(params?: LoginParams | Params): Observable<any>;

  private catchError = (err: HttpErrorResponse) => {
    this.httpErrorReporter.reportError(err);
    return of(null);
  };

  constructor(protected injector: Injector) {
    this.httpErrorReporter = injector.get(HttpErrorReporterService);
    this.environment = injector.get(EnvironmentService);
    this.configState = injector.get(ConfigStateService);
    this.oAuthService = injector.get(OAuthService2);
    this.sessionState = injector.get(SessionStateService);
    this.localStorageService = injector.get(AbpLocalStorageService);
    this.oAuthConfig = this.environment.getEnvironment().oAuthConfig || {};
    this.tenantKey = injector.get(TENANT_KEY);
    this.router = injector.get(Router);
    this.oAuthErrorFilterService = injector.get(OAuthErrorFilterService);
    this.rememberMeService = injector.get(RememberMeService);
    this.windowService = injector.get(AbpWindowService);

    this.listenToOauthErrors();
  }

  async init(): Promise<any> {
    if (this.oAuthConfig.clientId) {
      const shouldClear = shouldStorageClear(this.oAuthConfig.clientId, this.injector);
      if (shouldClear) clearOAuthStorage(this.injector);
    }
    this.oAuthService.configure(this.oAuthConfig);
    this.oAuthService.events
      .pipe(
        filter(
          event =>
            event.type === 'token_refresh_error' &&
            !isTransientTokenError((event as OAuthErrorEvent).reason),
        ),
      )
      // Through AuthService, so a sign-in that another request started isn't started again.
      .subscribe(() =>
        this.injector
          .get(AuthService)
          .navigateToLogin({ returnUrl: this.injector.get(Location).path(true) || '/' }),
      );
    this.navigateToPreviousUrl();
    return this.oAuthService
      .loadDiscoveryDocument()
      .then(() => {
        const isTokenExpire = isTokenExpired(this.oAuthService.getAccessTokenExpiration());
        if (isTokenExpire && this.oAuthService.getRefreshToken()) {
          return this.refreshToken();
        }

        return Promise.resolve();
      })
      .catch(this.catchError);
  }

  protected navigateToPreviousUrl(): void {
    const { responseType } = this.oAuthConfig;
    if (responseType === 'code') {
      this.oAuthService.events
        .pipe(
          filter(event => event.type === 'token_received' && !!this.oAuthService.state),
          take(1),
          map(() => {
            const redirectUri = decodeURIComponent(this.oAuthService.state);

            if (redirectUri && redirectUri !== '/') {
              return redirectUri;
            }

            return '/';
          }),
          switchMap(redirectUri =>
            this.configState.getOne$('currentUser').pipe(
              filter(user => !!user?.isAuthenticated),
              tap(() => this.router.navigateByUrl(redirectUri)),
            ),
          ),
        )
        .subscribe();
    }
  }

  protected refreshToken() {
    return this.oAuthService.refreshToken().catch(error => {
      if (!isTransientTokenError(error)) {
        clearOAuthStorage(this.injector);
      }
    });
  }

  // With tokens shared between tabs, a failed sign-in callback or discovery request of this tab
  // doesn't make the tokens of the other tabs invalid. Once this tab stored tokens of its own (for
  // example before their ID token failed the validation), everything is cleared as before.
  protected clearAfterError(err: OAuthErrorEvent) {
    // Without a connection or with a server error the tokens can still be valid; when they
    // expire, the route guard or the 401 handler signs in again.
    if (err.type === 'token_refresh_error' && isTransientTokenError(err.reason)) {
      return;
    }

    if (SIGN_IN_ERRORS.includes(err.type) && this.hasOnlySharedTokens()) {
      const storage = this.injector.get(OAuthStorage);
      ['nonce', 'PKCE_verifier', 'session_state'].forEach(key => storage.removeItem(key));
      return;
    }

    clearOAuthStorage(this.injector);
  }

  protected getMemoryStorage(): MemoryTokenStorageService | null {
    const storage = this.injector.get(OAuthStorage, null);
    return storage instanceof MemoryTokenStorageService ? storage : null;
  }

  // The tokens that other tabs keep in memory, without one this tab stored.
  protected hasOnlySharedTokens(): boolean {
    const storage = this.getMemoryStorage();
    return !!storage && !storage.hasOwnTokens;
  }

  protected listenToOauthErrors() {
    this.oAuthService.events
      .pipe(
        filter(event => event instanceof OAuthErrorEvent),
        tap((err: OAuthErrorEvent) => {
          const shouldSkip = this.oAuthErrorFilterService.run(err);
          if (!shouldSkip) {
            this.clearAfterError(err);
          }
        }),
        switchMap(() => this.configState.refreshAppState()),
      )
      .subscribe();
  }
}

// Errors of the sign-in of this tab (its callback or discovery). Others, like a failed token
// revocation at logout, clear the tokens as before.
const SIGN_IN_ERRORS: string[] = [
  'discovery_document_load_error',
  'code_error',
  'invalid_nonce_in_state',
  'jwks_load_error',
  'token_error',
  'token_validation_error',
];

function shouldStorageClear(clientId: string, injector: Injector): boolean {
  const storage = injector.get(OAuthStorage);
  const key = 'abpOAuthClientId';
  if (!storage.getItem(key)) {
    storage.setItem(key, clientId);
    return false;
  }

  const shouldClear = storage.getItem(key) !== clientId;
  if (shouldClear) storage.setItem(key, clientId);
  return shouldClear;
}
