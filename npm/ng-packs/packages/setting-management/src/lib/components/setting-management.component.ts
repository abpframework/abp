import { SettingTab } from '@abp/ng.theme.shared';
import { Component, TrackByFunction } from '@angular/core';
import { Router } from '@angular/router';
import { ABP, ConfigState } from '@abp/ng.core';
import { Store } from '@ngxs/store';

@Component({
  selector: 'abp-setting-management',
  templateUrl: './setting-management.component.html',
})
export class SettingManagementComponent {
  settings: SettingTab[] = [];

  selected = {} as SettingTab;

  trackByFn: TrackByFunction<SettingTab> = (_, item) => item.name;

  constructor(private router: Router, private store: Store) {}

  ngOnInit() {
    this.setSettings();
  }

  setSettings() {
    const { data } = this.router.config.find(r => r.path === 'setting-management') as {
      data: ABP.Dictionary<SettingTab>;
    };

    if (data) {
      Object.keys(data).forEach(key => {
        const element = data[key];
        if (element && element.component) {
          if (
            element.requiredPolicy &&
            !this.store.selectSnapshot(ConfigState.getGrantedPolicy(element.requiredPolicy))
          ) {
            return;
          }

          this.settings.push(element);
        }
      });

      if (this.settings.length) {
        this.settings = this.settings.sort((a, b) => a.order - b.order);
        this.selected = this.settings[0];
      }
    }
  }

  ngOnDestroy() {}
}
