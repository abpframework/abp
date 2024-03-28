import { AuthService } from '@abp/ng.core';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  protected readonly authService = inject(AuthService);

  loading = false;
  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  login() {
    this.loading = true;
    this.authService.navigateToLogin();
  }
}
