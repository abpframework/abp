import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';
import { ConfigState } from '../states/config.state';
import { CORE_OPTIONS } from '../tokens/options.token';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { RestService } from '../services/rest.service';
import { switchMap } from 'rxjs/operators';
import { GetAppConfiguration } from '../actions/config.actions';

export abstract class AuthFlowStrategy {
  abstract readonly isInternalAuth: boolean;

  protected oAuthService: OAuthService;
  protected oAuthConfig: AuthConfig;
  abstract checkIfInternalAuth(): boolean;
  abstract login(): void;
  abstract logout(): Observable<any>;
  abstract destroy(): void;

  private catchError = err => {
    // TODO: handle the error
  };

  constructor(protected injector: Injector) {
    this.oAuthService = injector.get(OAuthService);
    this.oAuthConfig = injector.get(CORE_OPTIONS).environment.oAuthConfig;
  }

  async init(): Promise<any> {
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
      .then(() => this.oAuthService.setupAutomaticSilentRefresh());
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

export class AuthPasswordFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = true;

  login() {
    const router = this.injector.get(Router);
    router.navigateByUrl('/account/login');
  }

  checkIfInternalAuth() {
    return true;
  }

  logout() {
    const store = this.injector.get(Store);
    const rest = this.injector.get(RestService);

    const issuer = store.selectSnapshot(ConfigState.getDeep('environment.oAuthConfig.issuer'));
    return rest
      .request(
        {
          method: 'GET',
          url: '/api/account/logout',
        },
        null,
        issuer,
      )
      .pipe(
        switchMap(() => {
          this.oAuthService.logOut();
          return store.dispatch(new GetAppConfiguration());
        }),
      );
  }

  destroy() {}
}

export const AUTH_FLOW_STRATEGY = {
  Code(injector: Injector) {
    return new AuthCodeFlowStrategy(injector);
  },
  Password(injector: Injector) {
    return new AuthPasswordFlowStrategy(injector);
  },
};
