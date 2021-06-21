import { Injectable } from '@angular/core';
import { CanActivate, UrlTree } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
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
