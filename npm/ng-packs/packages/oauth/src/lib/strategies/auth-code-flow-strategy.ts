import { noop } from '@abp/ng.core';
import { Params } from '@angular/router';
import { filter, from, of } from 'rxjs';
import { AuthFlowStrategy } from './auth-flow-strategy';
import { getRememberMe, removeRememberMe, setRememberMe } from '../utils';

export class AuthCodeFlowStrategy extends AuthFlowStrategy{
  readonly isInternalAuth = false;
  private remember_me = 'remember_me'

  async init() {
    this.checkRememberMeOption();

    return super
      .init()
      .then(() => this.oAuthService.tryLogin().catch(noop))
      .then(() => this.oAuthService.setupAutomaticSilentRefresh())
      // .then(() => this.listenToTokenExpiration());
  }

  // private listenToTokenExpiration() {
  //   this.oAuthService.events
  //     .pipe(
  //       filter(
  //         event => {
  //           return event instanceof OAuthInfoEvent &&
  //           event.type === 'token_expires' &&
  //           event.info === 'access_token'
  //         }
  //       ),
  //     )
  //     .subscribe(() => {
  //       if (this.oAuthService.getRefreshToken()) {
  //         console.log('refresh token');
  //         this.refreshToken();
  //       } else {
  //         this.oAuthService.logOut();
  //         removeRememberMe(this.localStorageService);
  //         this.configState.refreshAppState().subscribe();
  //       }
  //     });
  // }

  private checkRememberMeOption() {
    const accessToken = this.oAuthService.getAccessToken();
    const expireDate = this.oAuthService.getAccessTokenExpiration();
    const currentDate = new Date().getTime();
    let rememberMe = getRememberMe(this.localStorageService);

    if (accessToken && rememberMe === null) {
      let parsedToken = JSON.parse(atob(accessToken.split(".")[1]));

      if (parsedToken[this.remember_me]) {
        setRememberMe(true, this.localStorageService);
      } else {
        setRememberMe(false, this.localStorageService)
      }
      
    }
    rememberMe = getRememberMe(this.localStorageService);

    if (accessToken && expireDate < currentDate && rememberMe === 'false') {
      removeRememberMe(this.localStorageService);
      this.oAuthService.logOut();
    }else{
      console.log('try login');
      this.oAuthService.tryLogin().catch(noop)
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
