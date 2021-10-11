import { ApplicationInfo, EnvironmentService } from '@abp/ng.core';
import { Component } from '@angular/core';

@Component({
  selector: 'abp-logo',
  template: `
    <a class="navbar-brand" routerLink="/">
      <img
        *ngIf="appInfo.logoUrl; else appName"
        [src]="appInfo.logoUrl"
        [alt]="appInfo.name"
        width="100%"
        height="auto"
      />
    </a>

    <ng-template #appName>
      {{ appInfo.name }}
    </ng-template>
  `,
})
export class LogoComponent {
  get appInfo(): ApplicationInfo {
    return this.environment.getEnvironment().application;
  }

  constructor(private environment: EnvironmentService) {}
}
