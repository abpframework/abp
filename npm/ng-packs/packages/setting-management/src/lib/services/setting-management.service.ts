import { SettingTab } from '@abp/ng.theme.shared';
import { Injectable } from '@angular/core';
import { Router, RouteConfigLoadEnd, NavigationEnd } from '@angular/router';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { filter } from 'rxjs/operators';
import { takeUntilDestroy } from '@abp/ng.core';
import { Subscription, timer } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SettingManagementService {
  settings: SettingTab[] = [];

  selected = {} as SettingTab;

  constructor(private router: Router, private store: Store) {
    let timeout: Subscription;
    this.router.events
      .pipe(
        filter(event => event instanceof RouteConfigLoadEnd),
        takeUntilDestroy(this),
      )
      .subscribe(event => {
        if (timeout) timeout.unsubscribe();
        timeout = timer(150).subscribe(() => {
          this.setSettings();
        });
      });
  }

  ngOnDestroy() {}

  setSettings() {
    setTimeout(() => {
      const route = this.router.config.find(r => r.path === 'setting-management');
      this.settings = route.data.settings.sort((a, b) => a.order - b.order);
      this.checkSelected();
    }, 0);
  }

  checkSelected() {
    this.selected = this.settings.find(setting => setting.url === this.router.url) || ({} as SettingTab);

    if (!this.selected.name && this.settings.length) {
      this.setSelected(this.settings[0]);
    }
  }

  setSelected(selected: SettingTab) {
    this.selected = selected;
    this.store.dispatch(new Navigate([selected.url]));
  }
}
