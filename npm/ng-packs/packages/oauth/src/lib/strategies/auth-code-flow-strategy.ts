import { noop } from '@abp/ng.core';
import { Params } from '@angular/router';
import { from, of } from 'rxjs';
import { AuthFlowStrategy } from './auth-flow-strategy';

export class AuthCodeFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = false;

  async init() {
    return super
      .init()
      .then(() => this.oAuthService.tryLogin().catch(noop))
      .then(() => this.oAuthService.setupAutomaticSilentRefresh({}, 'access_token'));
  }

  navigateToLogin(queryParams?: Params) {
    this.oAuthService.initCodeFlow('', this.getCultureParams(queryParams));
  }

  checkIfInternalAuth(queryParams?: Params) {
    this.oAuthService.initCodeFlow('', this.getCultureParams(queryParams));
    return false;
  }

  logout(queryParams?: Params) {
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
