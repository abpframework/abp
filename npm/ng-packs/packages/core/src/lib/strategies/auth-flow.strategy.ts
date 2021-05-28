import { HttpHeaders } from '@angular/common/http';
import { Injector } from '@angular/core';
import { Params, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import {
  AuthConfig,
  OAuthErrorEvent,
  OAuthInfoEvent,
  OAuthService,
  OAuthStorage,
} from 'angular-oauth2-oidc';
import { from, Observable, of } from 'rxjs';
import { filter, switchMap, tap } from 'rxjs/operators';
import { RestOccurError } from '../actions/rest.actions';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { ConfigStateService } from '../services/config-state.service';
import { EnvironmentService } from '../services/environment.service';
import { SessionStateService } from '../services/session-state.service';
import { noop } from '../utils/common-utils';

export interface LoginParams {
  username: string;
  password: string;
  rememberMe?: boolean;
  redirectUrl?: string;
}

export const oAuthStorage = localStorage;

export abstract class AuthFlowStrategy {
  abstract readonly isInternalAuth: boolean;

  protected store: Store;
  protected environment: EnvironmentService;
  protected configState: ConfigStateService;
  protected oAuthService: OAuthService;
  protected oAuthConfig: AuthConfig;
  protected sessionState: SessionStateService;
  protected appConfigService: AbpApplicationConfigurationService;

  abstract checkIfInternalAuth(): boolean;
  abstract navigateToLogin(queryParams?: Params): void;
  abstract logout(): Observable<any>;
  abstract login(params?: LoginParams): Observable<any>;

  private catchError = err => this.store.dispatch(new RestOccurError(err));

  constructor(protected injector: Injector) {
    this.store = injector.get(Store);
    this.environment = injector.get(EnvironmentService);
    this.configState = injector.get(ConfigStateService);
    this.oAuthService = injector.get(OAuthService);
    this.appConfigService = injector.get(AbpApplicationConfigurationService);
    this.sessionState = injector.get(SessionStateService);
    this.oAuthConfig = this.environment.getEnvironment().oAuthConfig;

    this.listenToOauthErrors();
  }

  async init(): Promise<any> {
    const shouldClear = shouldStorageClear(
      this.environment.getEnvironment().oAuthConfig.clientId,
      oAuthStorage,
    );
    if (shouldClear) clearOAuthStorage(oAuthStorage);

    this.oAuthService.configure(this.oAuthConfig);
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
        switchMap(() => this.appConfigService.get()),
      )
      .subscribe(res => {
        this.configState.setState(res);
      });
  }
}

export class AuthCodeFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = false;

  async init() {
    return super
      .init()
      .then(() => this.oAuthService.tryLogin().catch(noop))
      .then(() => this.oAuthService.setupAutomaticSilentRefresh({}, 'access_token'));
  }

  navigateToLogin(queryParams?: Params) {
    const lang = this.sessionState.getLanguage();
    const culture = { culture: lang, 'ui-culture': lang };
    this.oAuthService.initCodeFlow(null, { ...(lang && culture), ...queryParams });
  }

  checkIfInternalAuth() {
    this.oAuthService.initCodeFlow();
    return false;
  }

  logout() {
    return from(this.oAuthService.revokeTokenAndLogout());
  }

  login() {
    this.oAuthService.initCodeFlow();
    return of(null);
  }
}

export class AuthPasswordFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = true;
  private cookieKey = 'rememberMe';
  private storageKey = 'passwordFlow';

  private listenToTokenExpiration() {
    this.oAuthService.events
      .pipe(
        filter(
          event =>
            event instanceof OAuthInfoEvent &&
            event.type === 'token_expires' &&
            event.info === 'access_token',
        ),
      )
      .subscribe(() => {
        if (this.oAuthService.getRefreshToken()) {
          this.refreshToken();
        } else {
          this.oAuthService.logOut();
          this.removeRememberMe();
          this.appConfigService.get().subscribe(res => {
            this.configState.setState(res);
          });
        }
      });
  }

  private setRememberMe(remember: boolean) {
    this.removeRememberMe();
    localStorage.setItem(this.storageKey, 'true');
    document.cookie = `${this.cookieKey}=true; path=/${
      remember ? ' ;expires=Fri, 31 Dec 9999 23:59:59 GMT' : ''
    }`;
  }

  private removeRememberMe() {
    localStorage.removeItem(this.storageKey);
    document.cookie = this.cookieKey + '= ; path=/; expires = Thu, 01 Jan 1970 00:00:00 GMT';
  }

  async init() {
    if (!getCookieValueByName(this.cookieKey) && localStorage.getItem(this.storageKey)) {
      this.oAuthService.logOut();
    }

    return super.init().then(() => this.listenToTokenExpiration());
  }

  navigateToLogin(queryParams?: Params) {
    const router = this.injector.get(Router);
    router.navigate(['/account/login'], { queryParams });
  }

  checkIfInternalAuth() {
    return true;
  }

  login(params: LoginParams): Observable<any> {
    const router = this.injector.get(Router);
    const tenant = this.sessionState.getTenant();

    return from(
      this.oAuthService.fetchTokenUsingPasswordFlow(
        params.username,
        params.password,
        new HttpHeaders({ ...(tenant && tenant.id && { __tenant: tenant.id }) }),
      ),
    ).pipe(
      switchMap(() => this.appConfigService.get()),
      tap(res => {
        this.configState.setState(res);
        this.setRememberMe(params.rememberMe);
        if (params.redirectUrl) router.navigate([params.redirectUrl]);
      }),
    );
  }

  logout() {
    const router = this.injector.get(Router);

    return from(this.oAuthService.revokeTokenAndLogout()).pipe(
      switchMap(() => this.appConfigService.get()),
      tap(res => {
        this.configState.setState(res);
        router.navigateByUrl('/');
        this.removeRememberMe();
      }),
    );
  }

  protected refreshToken() {
    return this.oAuthService.refreshToken().catch(() => {
      clearOAuthStorage();
      this.removeRememberMe();
    });
  }
}

export const AUTH_FLOW_STRATEGY = {
  Code(injector: Injector) {
    return new AuthCodeFlowStrategy(injector);
  },
  Password(injector: Injector) {
    return new AuthPasswordFlowStrategy(injector);
  },
};

export function clearOAuthStorage(storage: OAuthStorage = oAuthStorage) {
  const keys = [
    'access_token',
    'id_token',
    'refresh_token',
    'nonce',
    'PKCE_verifier',
    'expires_at',
    'id_token_claims_obj',
    'id_token_expires_at',
    'id_token_stored_at',
    'access_token_stored_at',
    'granted_scopes',
    'session_state',
  ];

  keys.forEach(key => storage.removeItem(key));
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

function getCookieValueByName(name: string) {
  const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
  return match ? match[2] : '';
}
