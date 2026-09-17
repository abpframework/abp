import { AuthService, LocalizationPipe } from '@abp/ng.core';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { ButtonComponent, CardBodyComponent, CardComponent } from '@abp/ng.theme.shared';
import { RouterLink } from '@angular/router';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
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
  readonly loading = signal(false);

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  login() {
    this.loading.set(true);
    this.authService.navigateToLogin();
  }
}
