import { noop } from '@abp/ng.core';
import { Params } from '@angular/router';
import { from, of } from 'rxjs';
import { AuthFlowStrategy } from './auth-flow-strategy';
import { RememberMeService, isTokenExpired } from '../utils';

export class AuthCodeFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = false;
  private rememberMe = 'remember_me'
  rememberMeService = new RememberMeService(this.injector);

  async init() {
    this.checkRememberMeOption();

    return super
      .init()
      .then(() => this.oAuthService.tryLogin().catch(noop))
      .then(() => this.oAuthService.setupAutomaticSilentRefresh())
  }

  private checkRememberMeOption() {
    const accessToken = this.oAuthService.getAccessToken();
    const isTokenExpire = isTokenExpired(this.oAuthService);
    let rememberMe = Boolean(JSON.parse(this.rememberMeService.getRememberMe()));

    if (accessToken && !rememberMe) {
      let parsedToken = JSON.parse(atob(accessToken.split(".")[1]));
      const rememberMeValue = Boolean(parsedToken[this.rememberMe]);

      if (rememberMeValue) {
        this.rememberMeService.setRememberMe(true);
      } else {
        this.rememberMeService.setRememberMe(false)
      }
    }

    rememberMe = Boolean(JSON.parse(this.rememberMeService.getRememberMe()));
    if (accessToken && isTokenExpire && !rememberMe) {
      this.rememberMeService.removeRememberMe();
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
    this.rememberMeService.removeRememberMe();
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
