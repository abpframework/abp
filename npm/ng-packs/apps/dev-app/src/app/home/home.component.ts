import { OAUTH_STRATEGY } from '@abp/ng.core';
import { Component, Injector } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(private oAuthService: OAuthService, private injector: Injector) {}

  login() {
    OAUTH_STRATEGY.NavigateToLogin(this.injector);
  }
}
