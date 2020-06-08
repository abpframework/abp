import { Config, ConfigState } from '@abp/ng.core';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';

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
  get appInfo(): Config.Application {
    return this.store.selectSnapshot(ConfigState.getApplicationInfo);
  }

  constructor(private store: Store) {}
}
