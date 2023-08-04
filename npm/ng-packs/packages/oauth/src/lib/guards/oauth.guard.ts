import { Injectable, inject } from '@angular/core';
import { UrlTree, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { Observable, delay, of, tap } from 'rxjs';
import { OAuthService } from 'angular-oauth2-oidc';

import { AuthService, HttpErrorReporterService, IAbpGuard } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class AbpOAuthGuard implements IAbpGuard {
  protected readonly oAuthService = inject(OAuthService);
  protected readonly authService = inject(AuthService);
  protected readonly httpErrorReporter = inject(HttpErrorReporterService);

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean> | boolean | UrlTree {
    const hasValidAccessToken = this.oAuthService.hasValidAccessToken();
    if (hasValidAccessToken) {
      return true;
    }
    const params = { returnUrl: state.url };
    this.authService.navigateToLogin(params);
    return false;
  }
}
