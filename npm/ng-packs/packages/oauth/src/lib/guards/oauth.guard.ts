import { Injectable, inject } from '@angular/core';
import { UrlTree } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { Observable } from 'rxjs';
import { OAuthService } from 'angular-oauth2-oidc';

import { AuthService, HttpErrorReporterService, IAbpGuard } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class AbpOAuthGuard implements IAbpGuard {
  protected readonly oAuthService = inject(OAuthService);
  protected readonly authService = inject(AuthService);
  protected readonly httpErrorReporter = inject(HttpErrorReporterService);

  canActivate(): Observable<boolean> | boolean | UrlTree {
    const hasValidAccessToken = this.oAuthService.hasValidAccessToken();
    if (hasValidAccessToken) {
      return true;
    }

    this.httpErrorReporter.reportError({ status: 401 } as HttpErrorResponse);
    return false;
  }
}
