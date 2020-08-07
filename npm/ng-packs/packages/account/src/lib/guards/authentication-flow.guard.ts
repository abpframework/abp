import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class AuthenticationFlowGuard implements CanActivate {
  constructor(private oauthService: OAuthService) {}

  canActivate() {
    if (this.oauthService.responseType === 'code') {
      this.oauthService.initCodeFlow();
      return false;
    }

    return true;
  }
}
