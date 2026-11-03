import { Injector } from '@angular/core';
import { CheckAuthenticationStateFn, ConfigStateService } from '@abp/ng.core';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { clearOAuthStorage } from './clear-o-auth-storage';
import { MemoryTokenStorageService } from '../services/memory-token-storage.service';

export const checkAccessToken: CheckAuthenticationStateFn = function (injector: Injector) {
  const configState = injector.get(ConfigStateService);
  const oAuth = injector.get(OAuthService);
  if (oAuth.hasValidAccessToken() && !configState.getDeep('currentUser.id')) {
    // hasValidAccessToken allows a clock skew; an expired token is expected to get an anonymous
    // configuration (for example after a refresh that couldn't reach the server), so it doesn't
    // mean the stored tokens are invalid. The route guard or the 401 handler signs in again.
    const expiration = oAuth.getAccessTokenExpiration();
    if (expiration && expiration <= Date.now()) {
      return;
    }

    // When the token came from another tab after the configuration request started, only this tab
    // is behind; clearing the shared storage would sign out every tab.
    const storage = injector.get(OAuthStorage, null);
    if (storage instanceof MemoryTokenStorageService && storage.handleTokensFromOtherTab()) {
      return;
    }

    clearOAuthStorage(injector);
  }
};
