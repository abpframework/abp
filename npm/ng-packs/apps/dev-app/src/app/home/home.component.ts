import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { timeInterval } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  isLog: boolean = false;
  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService) {}

  login() {
    this.isLog = true;
    setTimeout(data => {
      this.authService.navigateToLogin();
      this.isLog = false;
    }, 1000);
  }
}
