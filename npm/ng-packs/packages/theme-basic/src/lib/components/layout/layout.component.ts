import { Config, ConfigState } from '@abp/ng.core';
import { slideFromBottom } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';

@Component({
  selector: ' abp-layout',
  templateUrl: './layout.component.html',
  animations: [slideFromBottom]
})
export class LayoutComponent {
  isCollapsed = true;

  get appInfo(): Config.Application {
    return this.store.selectSnapshot(ConfigState.getApplicationInfo);
  }

  constructor(private store: Store) {}
}
