import { Injector } from '@angular/core';
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
  ConfigStateService,
  EnvironmentService,
  HttpErrorReporterService,
  LoginParams,
  SessionStateService,
  TENANT_KEY,
} from '@abp/ng.core';

import { clearOAuthStorage } from '../utils/clear-o-auth-storage';
import { oAuthStorage } from '../utils/oauth-storage';
import { OAuthErrorFilterService } from '../services';
import { isTokenExpired } from '../utils';
import { RememberMeService } from '../services/remember-me.service';

export abstract class AuthFlowStrategy {
  abstract readonly isInternalAuth: boolean;

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
      const shouldClear = shouldStorageClear(this.oAuthConfig.clientId, oAuthStorage);
      if (shouldClear) clearOAuthStorage(oAuthStorage);
    }
    this.oAuthService.configure(this.oAuthConfig);
    this.oAuthService.events
      .pipe(filter(event => event.type === 'token_refresh_error'))
      .subscribe(() => this.navigateToLogin());
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
    return this.oAuthService.refreshToken().catch(() => clearOAuthStorage());
  }

  protected listenToOauthErrors() {
    this.oAuthService.events
      .pipe(
        filter(event => event instanceof OAuthErrorEvent),
        tap((err: OAuthErrorEvent) => {
          const shouldSkip = this.oAuthErrorFilterService.run(err);
          if (!shouldSkip) {
            clearOAuthStorage();
          }
        }),
        switchMap(() => this.configState.refreshAppState()),
      )
      .subscribe();
  }
}

function shouldStorageClear(clientId: string, storage: OAuthStorage): boolean {
  const key = 'abpOAuthClientId';
  if (!storage.getItem(key)) {
    storage.setItem(key, clientId);
    return false;
  }

  const shouldClear = storage.getItem(key) !== clientId;
  if (shouldClear) storage.setItem(key, clientId);
  return shouldClear;
}
