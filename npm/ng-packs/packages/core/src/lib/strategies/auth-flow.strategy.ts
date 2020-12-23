import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { AuthConfig, OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { Observable, of } from 'rxjs';
import { RestOccurError } from '../actions/rest.actions';
import { ConfigStateService } from '../services/config-state.service';
import { EnvironmentService } from '../services/environment.service';

export const oAuthStorage = localStorage;

export abstract class AuthFlowStrategy {
  abstract readonly isInternalAuth: boolean;

  protected store: Store;
  protected environment: EnvironmentService;
  protected configState: ConfigStateService;
  protected oAuthService: OAuthService;
  protected oAuthConfig: AuthConfig;
  abstract checkIfInternalAuth(): boolean;
  abstract login(): void;
  abstract logout(): Observable<any>;
  abstract destroy(): void;

  private catchError = err => this.store.dispatch(new RestOccurError(err));

  constructor(protected injector: Injector) {
    this.store = injector.get(Store);
    this.environment = injector.get(EnvironmentService);
    this.configState = injector.get(ConfigStateService);
    this.oAuthService = injector.get(OAuthService);
    this.oAuthConfig = this.environment.getEnvironment().oAuthConfig;
  }

  async init(): Promise<any> {
    const shouldClear = shouldStorageClear(
      this.environment.getEnvironment().oAuthConfig.clientId,
      oAuthStorage,
    );
    if (shouldClear) clearOAuthStorage(oAuthStorage);

    this.oAuthService.configure(this.oAuthConfig);
    return this.oAuthService.loadDiscoveryDocument().catch(this.catchError);
  }
}

export class AuthCodeFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = false;

  async init() {
    return super
      .init()
      .then(() => this.oAuthService.tryLogin())
      .then(() => {
        if (this.oAuthService.hasValidAccessToken() || !this.oAuthService.getRefreshToken()) {
          return Promise.resolve();
        }

        return this.oAuthService.refreshToken() as Promise<any>;
      })
      .then(() => this.oAuthService.setupAutomaticSilentRefresh({}, 'access_token'));
  }

  login() {
    this.oAuthService.initCodeFlow();
  }

  checkIfInternalAuth() {
    this.oAuthService.initCodeFlow();
    return false;
  }

  logout() {
    this.oAuthService.logOut();
    return of(null);
  }

  destroy() {}
}

export const AUTH_FLOW_STRATEGY = {
  Code(injector: Injector) {
    return new AuthCodeFlowStrategy(injector);
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
