import { Component, TrackByFunction, OnInit } from '@angular/core';
import { SettingTab, SETTING_TABS } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';

@Component({
  selector: 'abp-setting-management',
  templateUrl: './setting-management.component.html',
})
export class SettingManagementComponent implements OnInit {
  settings: SettingTab[] = [];

  selected = {} as SettingTab;

  trackByFn: TrackByFunction<SettingTab> = (_, item) => item.name;

  constructor(private router: Router, private store: Store) {}

  ngOnInit() {
    if (this.settings.length) {
      this.settings = SETTING_TABS.filter(setting =>
        this.store.selectSnapshot(ConfigState.getGrantedPolicy(setting.requiredPolicy)),
      ).sort((a, b) => a.order - b.order);
      this.selected = this.settings[0];
    }
  }
}
