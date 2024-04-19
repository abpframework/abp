import { ApplicationInfo, EnvironmentService } from '@abp/ng.core';
import { Component } from '@angular/core';

@Component({
  selector: 'abp-logo',
  template: `
    <a class="navbar-brand" routerLink="/">
      @if (appInfo.logoUrl) {
        <img
          [src]="appInfo.logoUrl"
          [alt]="appInfo.name"
          width="100%"
          height="auto"
        />
      } @else {
        {{ appInfo.name }}
      }
    </a>
  `,
})
export class LogoComponent {
  get appInfo(): ApplicationInfo {
    return this.environment.getEnvironment().application;
  }

  constructor(private environment: EnvironmentService) {}
}
