import { filter, switchMap, tap } from 'rxjs/operators';
import { OAuthInfoEvent } from 'angular-oauth2-oidc';
import { Params, Router } from '@angular/router';
import { from, Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { AuthFlowStrategy } from './auth-flow-strategy';
import { isTokenExpired, pipeToLogin } from '../utils/auth-utils';
import { LoginParams } from '@abp/ng.core';
import { clearOAuthStorage } from '../utils/clear-o-auth-storage';

export class AuthPasswordFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = true;

  private listenToTokenExpiration() {
    this.oAuthService.events
      .pipe(
        filter(
          event => event instanceof OAuthInfoEvent &&
            event.type === 'token_expires' &&
            event.info === 'access_token'
        ),
      )
      .subscribe(() => {
        if (this.oAuthService.getRefreshToken()) {
          this.refreshToken();
        } else {
          this.oAuthService.logOut();
          this.rememberMeService.remove();
          this.configState.refreshAppState().subscribe();
        }
      });
  }

  async init() {
    this.checkRememberMeOption();

    return super.init().then(() => this.listenToTokenExpiration());
  }

  private checkRememberMeOption() {
    const accessToken = this.oAuthService.getAccessToken();
    const isTokenExpire = isTokenExpired(this.oAuthService.getAccessTokenExpiration());
    const rememberMe = this.rememberMeService.get();
    if (accessToken && isTokenExpire && !rememberMe) {
      this.rememberMeService.remove();
      this.oAuthService.logOut();
    }
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

  logout() {
    const router = this.injector.get(Router);
    const noRedirectToLogoutUrl = true;
    return from(this.oAuthService.revokeTokenAndLogout(noRedirectToLogoutUrl)).pipe(
      switchMap(() => this.configState.refreshAppState()),
      tap(() => {
        this.rememberMeService.remove();
        router.navigateByUrl('/');
      }),
    );
  }

  protected refreshToken() {
    return this.oAuthService.refreshToken().catch(() => {
      clearOAuthStorage();
      this.rememberMeService.remove();
    });
  }
}
