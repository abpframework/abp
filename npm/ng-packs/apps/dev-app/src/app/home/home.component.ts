import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  isNavigating: boolean = false;

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  get iconClass(): string {
    return `fa ${this.isNavigating ? 'fa-spinner fa-pulse abp-loading' : 'fa-sign-in'}`;
  }

  get title(): string {
    return `AbpAccount::${this.isNavigating ? 'Redirecting' : 'Login'}`;
  }

  constructor(private authService: AuthService) {}

  login() {
    this.isNavigating = true;
    this.authService.navigateToLogin();
  }
}
