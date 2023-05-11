import { Injectable } from '@angular/core';
import { UrlTree } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { AuthService, IAuthGuard } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class AbpOAuthGuard  implements IAuthGuard {
  constructor(private oauthService: OAuthService, private authService: AuthService) {}

  canActivate(): Observable<boolean> | boolean | UrlTree {
    const hasValidAccessToken = this.oauthService.hasValidAccessToken();
    if (hasValidAccessToken) {
      return true;
    }

    this.authService.navigateToLogin();
    return false;
  }
}
