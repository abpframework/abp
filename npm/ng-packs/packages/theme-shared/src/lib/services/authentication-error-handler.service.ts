import { inject, Injectable } from '@angular/core';
import { Location } from '@angular/common';
import { ApplicationConfigurationDto, AuthService, ConfigStateService } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { filter, switchMap } from 'rxjs';
import { CustomHttpErrorHandlerService } from '../models/common';
import { CUSTOM_HTTP_ERROR_HANDLER_PRIORITY } from '../constants/default-errors';
import { HTTP_ERROR_CONFIG } from '../tokens/http-error.token';

@Injectable({ providedIn: 'root' })
export class AbpAuthenticationErrorHandler implements CustomHttpErrorHandlerService {
  readonly priority = CUSTOM_HTTP_ERROR_HANDLER_PRIORITY.veryHigh;
  protected readonly authService = inject(AuthService);
  protected readonly configStateService = inject(ConfigStateService);
  protected readonly location = inject(Location);
  protected readonly httpErrorConfig = inject(HTTP_ERROR_CONFIG, { optional: true });

  canHandle(error: unknown): boolean {
    return error instanceof HttpErrorResponse && error.status === 401;
  }

  execute() {
    // A token that expired before it was refreshed belongs to a signed-in user, so the user signs
    // in again and comes back to the current page. Without a token, that is opt-in.
    const accessToken = this.authService.getAccessToken();
    if (accessToken ? this.isTokenExpired() : this.httpErrorConfig?.signInWithoutToken) {
      // The app URL, without the base href, as the sign-in callback passes it to the router.
      this.authService.navigateToLogin({ returnUrl: this.location.path(true) || '/' });
      return;
    }

    // If another tab replaced the token meanwhile, an anonymous result is about the old token, so
    // the new one isn't signed out.
    const isAnonymous = ({ currentUser }: ApplicationConfigurationDto) =>
      !currentUser.isAuthenticated && this.authService.getAccessToken() === accessToken;

    let anonymous$ = this.configStateService.refreshAppState().pipe(filter(isAnonymous));
    if (accessToken) {
      // The configuration can come from a request that an older token started before this one,
      // so it is loaded again before signing out.
      anonymous$ = anonymous$.pipe(
        switchMap(() => this.configStateService.refreshAppState()),
        filter(isAnonymous),
      );
    }

    anonymous$.subscribe(() => this.authService.logout({ noRedirectToLogoutUrl: true }));
  }

  // isAuthenticated allows a clock skew (10 minutes by default), but the API already rejected the
  // token here.
  private isTokenExpired(): boolean {
    const expiration = this.authService.getAccessTokenExpiration();
    return expiration > 0 && expiration <= Date.now();
  }
}
