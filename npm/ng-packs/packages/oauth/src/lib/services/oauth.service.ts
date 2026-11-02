import { Injectable, Injector, inject, PLATFORM_ID, DOCUMENT } from '@angular/core';
import { Params } from '@angular/router';
import { from, Observable, lastValueFrom, EMPTY } from 'rxjs';
import { filter, map, switchMap, take, tap } from 'rxjs/operators';
import {
  AbpAuthResponse,
  APP_STARTED_WITH_SSR,
  CORE_OPTIONS,
  IAuthService,
  LoginParams,
} from '@abp/ng.core';
import { AuthFlowStrategy } from '../strategies';
import { EnvironmentService } from '@abp/ng.core';
import { AUTH_FLOW_STRATEGY } from '../tokens/auth-flow-strategy';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { HttpHeaders } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { MemoryTokenStorageService } from './memory-token-storage.service';

function isPlainSignIn(queryParams?: Params): boolean {
  return Object.keys(queryParams ?? {}).every(key => key === 'returnUrl');
}

interface QueuedLogin {
  queryParams?: Params;
  // Whether the tab had a token when the sign-in was requested.
  hadToken: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class AbpOAuthService implements IAuthService {
  protected injector = inject(Injector);
  private appStartedWithSsr = this.injector.get(APP_STARTED_WITH_SSR);
  private platformId = this.injector.get(PLATFORM_ID);
  private document = this.injector.get(DOCUMENT);

  private strategy!: AuthFlowStrategy;
  private readonly oAuthService: OAuthService;
  private resolveInitialized!: () => void;
  // Completes when init() finishes. A 401 can arrive before init() is even called.
  private readonly initialized = new Promise<void>(resolve => (this.resolveInitialized = resolve));
  private isInitialized = false;
  // Sign-ins requested before init() finished, started when it finishes.
  private queuedLogins: QueuedLogin[] = [];
  // The code flow redirect in progress; the page leaves for the login page.
  private loginStarted?: Promise<void>;

  get oidc() {
    return this.oAuthService.oidc;
  }

  set oidc(value) {
    this.oAuthService.oidc = value;
  }

  get isInternalAuth() {
    return this.strategy.isInternalAuth;
  }

  constructor() {
    this.oAuthService = this.injector.get(OAuthService);

    // init() is never called then, so sign-in requests don't wait for it.
    if (this.injector.get(CORE_OPTIONS, null)?.skipInitAuthService) {
      this.isInitialized = true;
      this.resolveInitialized();
    }
  }

  async init() {
    try {
      // Load the tokens that other tabs keep in memory before the app reads them.
      const memoryStorage = this.getMemoryStorage();
      if (memoryStorage) {
        await memoryStorage.ready;
      }

      const environmentService = this.injector.get(EnvironmentService);

      const result$ = environmentService.getEnvironment$().pipe(
        map(env => env?.oAuthConfig),
        filter(Boolean),
        tap(oAuthConfig => {
          this.strategy =
            oAuthConfig.responseType === 'code'
              ? AUTH_FLOW_STRATEGY.Code(this.injector)
              : AUTH_FLOW_STRATEGY.Password(this.injector);
        }),
        switchMap(() => from(this.strategy.init())),
        take(1),
      );

      return await lastValueFrom(result$);
    } finally {
      this.isInitialized = true;
      // A sign-in callback of this page, if any, is over; a sign-in started meanwhile keeps the mark.
      if (!this.queuedLogins.length && !this.loginStarted) {
        this.getMemoryStorage()?.markSignInFinished();
      }
      this.resolveInitialized();
    }
  }

  logout(queryParams?: Params): Observable<any> {
    if (!this.strategy) {
      return EMPTY;
    }
    return this.strategy.logout(queryParams);
  }

  navigateToLogin(queryParams?: Params) {
    // Other ways to sign in get every request as soon as the strategy exists, as before.
    if (this.isInitialized || (this.strategy && !this.strategy.usesDefaultRedirect())) {
      this.startLogin(queryParams);
      return;
    }

    // A route guard and a 401 can ask for the sign-in before init() finishes; they wait for it.
    this.queuedLogins.push({ queryParams, hadToken: this.hasUnexpiredToken() });
    if (this.queuedLogins.length > 1) {
      return;
    }

    this.getMemoryStorage()?.markSignInStarted();
    this.initialized.then(() => this.startQueuedLogins());
  }

  private startQueuedLogins() {
    const logins = this.queuedLogins;
    this.queuedLogins = [];

    // The sign-in callback of this page, or another tab, can give a token while the default redirect
    // waits for init; a guard or a 401 then doesn't need the sign-in. Requests with their own
    // parameters (prompt, login_hint, ...) still run.
    const signedIn = this.strategy?.usesDefaultRedirect() && this.hasUnexpiredToken();
    const toStart = signedIn
      ? logins.filter(login => login.hadToken || !isPlainSignIn(login.queryParams))
      : logins;
    if (!toStart.length) {
      this.getMemoryStorage()?.markSignInFinished();
      return;
    }

    toStart.forEach(login => this.startLogin(login.queryParams));
  }

  // isAuthenticated allows a clock skew, but an expired token still needs the sign-in.
  private hasUnexpiredToken(): boolean {
    return this.isAuthenticated && this.getAccessTokenExpiration() > Date.now();
  }

  // The code flow strategy lets parallel requests share one redirect; other ways to sign in get
  // every request, as before.
  private startLogin(queryParams?: Params) {
    const storage = this.getMemoryStorage();
    let started: Promise<void> | undefined;
    try {
      started = this.strategy?.startLogin(queryParams);
    } catch (error) {
      storage?.markSignInFinished();
      console.error('Could not start the sign-in', error);
      return;
    }

    if (!started) {
      storage?.markSignInFinished();
      return;
    }

    storage?.markSignInStarted();
    if (started === this.loginStarted) {
      return;
    }

    const login = (this.loginStarted = started);
    const finish = () => {
      if (this.loginStarted === login) {
        this.loginStarted = undefined;
      }
      storage?.markSignInFinished();
    };
    // It settles when the page is still here after the redirect.
    login.then(finish, error => {
      finish();
      console.error('Could not start the sign-in', error);
    });
  }

  private getMemoryStorage(): MemoryTokenStorageService | null {
    const storage = this.injector.get(OAuthStorage, null);
    return storage instanceof MemoryTokenStorageService ? storage : null;
  }

  login(params: LoginParams) {
    return this.strategy.login(params);
  }

  get isAuthenticated(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  loginUsingGrant(
    grantType: string,
    parameters: object,
    headers?: HttpHeaders,
  ): Promise<AbpAuthResponse> {
    const { clientId: client_id, dummyClientSecret: client_secret } = this.oAuthService;
    const access_token = this.oAuthService.getAccessToken();
    const p = {
      access_token,
      grant_type: grantType,
      client_id,
      ...parameters,
    };

    if (client_secret) {
      p['client_secret'] = client_secret;
    }

    return this.oAuthService.fetchTokenUsingGrant(grantType, p, headers);
  }

  getRefreshToken(): string {
    return this.oAuthService.getRefreshToken();
  }

  getAccessToken(): string {
    return this.oAuthService.getAccessToken();
  }

  refreshToken(): Promise<AbpAuthResponse | void> {
    if (isPlatformBrowser(this.platformId) && this.appStartedWithSsr) {
      this.document.defaultView?.location.replace('/authorize');
      return Promise.resolve();
    }
    try {
      return this.oAuthService.refreshToken();
    } catch (error) {
      console.log("Error while refreshing token: ", error);
      return Promise.reject();
    }
  }

  getAccessTokenExpiration(): number {
    return this.oAuthService.getAccessTokenExpiration();
  }
}
