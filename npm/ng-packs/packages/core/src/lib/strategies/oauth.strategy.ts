import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { AuthConfig, OAuthService, OAuthSuccessEvent } from 'angular-oauth2-oidc';
import { ConfigState } from '../states/config.state';
import { CORE_OPTIONS } from '../tokens/options.token';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { RestService } from '../services/rest.service';
import { switchMap } from 'rxjs/operators';
import { GetAppConfiguration } from '../actions/config.actions';

export abstract class OAuthStrategy {
  protected oAuthService: OAuthService;
  protected oAuthConfig: AuthConfig;
  abstract navigateToLogin(): void;
  abstract canActivate(): boolean;
  abstract logOut(): Observable<any>;

  private catchError = err => {
    // TODO: handle the error
  };

  constructor(protected injector: Injector) {
    this.oAuthService = injector.get(OAuthService);
    this.oAuthConfig = injector.get(CORE_OPTIONS).environment.oAuthConfig;
  }

  async init(): Promise<OAuthSuccessEvent | void> {
    this.oAuthService.configure(this.oAuthConfig);
    return this.oAuthService.loadDiscoveryDocument().catch(this.catchError);
  }
}

export class OAuthCodeFlowStrategy extends OAuthStrategy {
  async init() {
    return super
      .init()
      .then(() => this.oAuthService.tryLogin())
      .then(() => this.oAuthService.setupAutomaticSilentRefresh());
  }

  navigateToLogin() {
    this.oAuthService.initCodeFlow();
  }

  canActivate() {
    this.oAuthService.initCodeFlow();
    return false;
  }

  logOut() {
    this.oAuthService.logOut();
    return of(null);
  }
}

export class OAuthPasswordFlowStrategy extends OAuthStrategy {
  navigateToLogin() {
    const router = this.injector.get(Router);
    router.navigateByUrl('/account/login');
  }

  canActivate() {
    return true;
  }

  logOut() {
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
}

export const OAUTH_STRATEGY = {
  async Init(injector: Injector) {
    return getOAuthStrategy(injector).init();
  },
  NavigateToLogin(injector: Injector) {
    return getOAuthStrategy(injector).navigateToLogin();
  },
  CanActivate(injector: Injector) {
    return getOAuthStrategy(injector).canActivate();
  },
  LogOut(injector: Injector) {
    return getOAuthStrategy(injector).logOut();
  },
};

function getOAuthStrategy(injector: Injector) {
  const codeFlow =
    injector
      .get(Store)
      .selectSnapshot(ConfigState.getDeep('environment.oAuthConfig.responseType')) === 'code';

  return codeFlow ? new OAuthCodeFlowStrategy(injector) : new OAuthPasswordFlowStrategy(injector);
}
