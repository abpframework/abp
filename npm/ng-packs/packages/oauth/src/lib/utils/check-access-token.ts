import { Injector } from '@angular/core';
import { CheckAuthenticationStateFn, ConfigStateService } from '@abp/ng.core';
import { OAuthService } from 'angular-oauth2-oidc';
import { clearOAuthStorage } from './clear-o-auth-storage';

export const checkAccessToken: CheckAuthenticationStateFn = function (injector: Injector) {
  const configState = injector.get(ConfigStateService);
  const oAuth = injector.get(OAuthService);
  if (oAuth.hasValidAccessToken() && !configState.getDeep('currentUser.id')) {
    clearOAuthStorage();
  }
};
