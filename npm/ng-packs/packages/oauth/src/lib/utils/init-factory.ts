import { ABP, AuthService, ConfigStateService, CORE_OPTIONS } from '@abp/ng.core';
import { Injector } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { clearOAuthStorage } from './clear-o-auth-storage';
import { lastValueFrom } from 'rxjs';
import { tap } from 'rxjs/operators';

export function initFactory(injector: Injector): () => Promise<void> {
  return async () => {
    const authService = injector.get(AuthService);
    await authService.init();
    const configState = injector.get(ConfigStateService);

    const options = injector.get(CORE_OPTIONS) as ABP.Root;

    if (options.skipGetAppConfiguration) {
      return;
    }

    const result$ = configState.refreshAppState().pipe(tap(() => checkAccessToken(injector)));
    await lastValueFrom(result$);
  };
}

export function checkAccessToken(injector: Injector) {
  const configState = injector.get(ConfigStateService);
  const oAuth = injector.get(OAuthService);
  if (oAuth.hasValidAccessToken() && !configState.getDeep('currentUser.id')) {
    clearOAuthStorage();
  }
}
