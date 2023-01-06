import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { map } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  loading: boolean = false;
  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService) {}
  login() {
    this.loading = true;
    this.authService.navigateToLogin();
  }
}
