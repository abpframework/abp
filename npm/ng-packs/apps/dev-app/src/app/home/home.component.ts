import { AuthService, LocalizationPipe } from '@abp/ng.core';
import { Component, inject } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { ButtonComponent, CardBodyComponent, CardComponent } from '@abp/ng.theme.shared';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  imports: [
    NgTemplateOutlet,
    LocalizationPipe,
    CardComponent,
    CardBodyComponent,
    ButtonComponent,
    RouterLink
  ],
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
