import { EnvironmentService } from '@abp/ng.core';
import { RouterLink } from '@angular/router';
import { Component, inject } from '@angular/core';
import { LOGO_APP_NAME_TOKEN, LOGO_URL_TOKEN } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-logo',
  template: `
    <a class="navbar-brand" routerLink="/">
      @if (logoUrl) {
        <img [src]="logoUrl" [alt]="appName" width="100%" height="auto" />
      } @else {
        {{ appName }}
      }
    </a>
  `,
  standalone: true,
  imports: [RouterLink],
})
export class LogoComponent {
  private environment = inject(EnvironmentService);

  private readonly providedLogoUrl = inject(LOGO_URL_TOKEN, { optional: true });
  private readonly providedAppName = inject(LOGO_APP_NAME_TOKEN, { optional: true });

  get logoUrl(): string {
    return (
      this.providedLogoUrl ?? this.environment.getEnvironment().application?.logoUrl
    );
  }

  get appName(): string {
    return (
      this.providedAppName ?? this.environment.getEnvironment().application?.name
    );
  }
}
