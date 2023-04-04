import { Injector } from '@angular/core';
import { Params } from '@angular/router';
import {
  AuthConfig,
  OAuthErrorEvent,
  OAuthService as OAuthService2,
  OAuthStorage,
} from 'angular-oauth2-oidc';
import { Observable, of } from 'rxjs';
import { filter, switchMap, tap } from 'rxjs/operators';
import {
  ConfigStateService,
  EnvironmentService,
  HttpErrorReporterService,
  LoginParams,
  SessionStateService,
  TENANT_KEY,
} from '@abp/ng.core';
import { clearOAuthStorage } from '../utils/clear-o-auth-storage';
import { oAuthStorage } from '../utils/oauth-storage';
import { HttpErrorResponse } from '@angular/common/http';

export abstract class AuthFlowStrategy {
  abstract readonly isInternalAuth: boolean;

  protected httpErrorReporter: HttpErrorReporterService;
  protected environment: EnvironmentService;
  protected configState: ConfigStateService;
  protected oAuthService: OAuthService2;
  protected oAuthConfig!: AuthConfig;
  protected sessionState: SessionStateService;
  protected tenantKey: string;

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
    this.oAuthConfig = this.environment.getEnvironment().oAuthConfig || {};
    this.tenantKey = injector.get(TENANT_KEY);

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

    return this.oAuthService
      .loadDiscoveryDocument()
      .then(() => {
        if (this.oAuthService.hasValidAccessToken() || !this.oAuthService.getRefreshToken()) {
          return Promise.resolve();
        }

        return this.refreshToken();
      })
      .catch(this.catchError);
  }

  protected refreshToken() {
    return this.oAuthService.refreshToken().catch(() => clearOAuthStorage());
  }

  protected listenToOauthErrors() {
    this.oAuthService.events
      .pipe(
        filter(event => event instanceof OAuthErrorEvent),
        tap(() => clearOAuthStorage()),
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
