import { filter, switchMap, tap } from 'rxjs/operators';
import { OAuthInfoEvent } from 'angular-oauth2-oidc';
import { Params, Router } from '@angular/router';
import { from, Observable, pipe } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { AuthFlowStrategy } from './auth-flow-strategy';
import { pipeToLogin, removeRememberMe, setRememberMe } from '../utils/auth-utils';
import { LoginParams } from '@abp/ng.core';
import { clearOAuthStorage } from '../utils/clear-o-auth-storage';

function getCookieValueByName(name: string) {
  const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
  return match ? match[2] : '';
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
          removeRememberMe();
          this.configState.refreshAppState().subscribe();
        }
      });
  }

  async init() {
    if (!getCookieValueByName(this.cookieKey) && localStorage.getItem(this.storageKey)) {
      this.oAuthService.logOut();
    }

    return super.init().then(() => this.listenToTokenExpiration());
  }

  navigateToLogin(queryParams?: Params) {
    const router = this.injector.get(Router);
    return router.navigate(['/account/login'], { queryParams });
  }

  checkIfInternalAuth() {
    return true;
  }

  login(params: LoginParams): Observable<any> {
    const tenant = this.sessionState.getTenant();

    return from(
      this.oAuthService.fetchTokenUsingPasswordFlow(
        params.username,
        params.password,
        new HttpHeaders({ ...(tenant && tenant.id && { [this.tenantKey]: tenant.id }) }),
      ),
    ).pipe(pipeToLogin(params, this.injector));
  }
  logout(queryParams?: Params) {
    const router = this.injector.get(Router);

    return from(this.oAuthService.revokeTokenAndLogout(queryParams)).pipe(
      switchMap(() => this.configState.refreshAppState()),
      tap(() => {
        router.navigateByUrl('/');
        removeRememberMe();
      }),
    );
  }

  protected refreshToken() {
    return this.oAuthService.refreshToken().catch(() => {
      clearOAuthStorage();
      removeRememberMe();
    });
  }
}
