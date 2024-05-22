import { noop } from '@abp/ng.core';
import { Params } from '@angular/router';
import { from, of } from 'rxjs';
import { AuthFlowStrategy } from './auth-flow-strategy';
import { isTokenExpired } from '../utils';

export class AuthCodeFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = false;

  async init() {
    this.checkRememberMeOption();

    return super
      .init()
      .then(() => this.oAuthService.tryLogin().catch(noop))
      .then(() => this.oAuthService.setupAutomaticSilentRefresh());
  }

  private checkRememberMeOption() {
    const accessToken = this.oAuthService.getAccessToken();
    const isTokenExpire = isTokenExpired(this.oAuthService.getAccessTokenExpiration());
    let rememberMe = this.rememberMeService.get();

    if (accessToken && !rememberMe) {
      const rememberMeValue = this.rememberMeService.getFromToken(accessToken);

      this.rememberMeService.set(!!rememberMeValue);
    }

    rememberMe = this.rememberMeService.get();
    if (accessToken && isTokenExpire && !rememberMe) {
      this.rememberMeService.remove();
      this.oAuthService.logOut();
    }
  }

  navigateToLogin(queryParams?: Params) {
    let additionalState = '';
    if (queryParams?.returnUrl) {
      additionalState = queryParams.returnUrl;
    }

    const cultureParams = this.getCultureParams(queryParams);
    this.oAuthService.initCodeFlow(additionalState, cultureParams);
  }

  checkIfInternalAuth(queryParams?: Params) {
    this.oAuthService.initCodeFlow('', this.getCultureParams(queryParams));
    return false;
  }

  logout(queryParams?: Params) {
    this.rememberMeService.remove();
    if (queryParams?.noRedirectToLogoutUrl) {
      this.router.navigate(['/']);
      return from(this.oAuthService.revokeTokenAndLogout(true));
    }
    return from(this.oAuthService.revokeTokenAndLogout(this.getCultureParams(queryParams)));
  }

  login(queryParams?: Params) {
    this.oAuthService.initCodeFlow('', this.getCultureParams(queryParams));
    return of(null);
  }

  private getCultureParams(queryParams?: Params) {
    const lang = this.sessionState.getLanguage();
    const culture = { culture: lang, 'ui-culture': lang };
    return { ...(lang && culture), ...queryParams };
  }
}
