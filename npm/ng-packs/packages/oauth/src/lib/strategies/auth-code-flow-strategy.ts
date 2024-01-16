import { noop } from '@abp/ng.core';
import { Params } from '@angular/router';
import { from, of } from 'rxjs';
import { AuthFlowStrategy } from './auth-flow-strategy';
import { getRememberMe, removeRememberMe, setRememberMe } from '../utils';

export class AuthCodeFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = false;
  private remember_me = 'remember_me'

  async init() {
    this.checkRememberMeOption();

    return super
      .init()
      .then(() => this.oAuthService.tryLogin().catch(noop))
      .then(() => this.oAuthService.setupAutomaticSilentRefresh())
  }

  private checkRememberMeOption() {
    const accessToken = this.oAuthService.getAccessToken();
    const expireDate = this.oAuthService.getAccessTokenExpiration();
    const currentDate = new Date().getTime();
    let rememberMe = getRememberMe(this.localStorageService);

    if (accessToken && rememberMe === null) {
      let parsedToken = JSON.parse(atob(accessToken.split(".")[1]));
      let rememberMeValue = parsedToken[this.remember_me];

      if (rememberMeValue && (rememberMeValue === 'True' || rememberMeValue === 'true')) {
        setRememberMe(true, this.localStorageService);
      } else {
        setRememberMe(false, this.localStorageService)
      }
    }

    rememberMe = getRememberMe(this.localStorageService);
    if (accessToken && expireDate < currentDate && rememberMe !== 'true') {
      removeRememberMe(this.localStorageService);
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
    removeRememberMe(this.localStorageService);
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
