import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'abp-home',
  template: `
    <div class="card">
      <div class="card-header">Welcome</div>
      <div class="card-body">
        <p>
          Welcome to the application. This is a startup project based on the ABP framework. For more information, visit
          abp.io.
        </p>
        <p *ngIf="!hasLoggedIn">
          <a routerLink="/account/login" [state]="{ redirectUrl: '/home' }" class="btn btn-primary" role="button"
            ><i class="fa fa-sign-in"></i>{{ 'AbpIdentity::Login' | abpLocalization }}</a
          >
        </p>
        <hr />
        <p class="text-right"><a href="https://abp.io?ref=tmpl" target="_blank">abp.io</a></p>
      </div>
    </div>
  `,
})
export class HomeComponent {
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(private oAuthService: OAuthService) {}
}
