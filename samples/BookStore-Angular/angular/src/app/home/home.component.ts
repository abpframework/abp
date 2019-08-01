import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'abp-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(private oAuthService: OAuthService) {}
}
