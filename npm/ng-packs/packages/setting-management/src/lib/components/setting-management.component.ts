import { Component, TrackByFunction, OnInit } from '@angular/core';
import { SettingTab, getSettingTabs } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';
import { SettingManagementState } from '../states/setting-management.state';
import { SetSelectedSettingTab } from '../actions/setting-management.actions';
import { RouterState } from '@ngxs/router-plugin';

@Component({
  selector: 'abp-setting-management',
  templateUrl: './setting-management.component.html',
})
export class SettingManagementComponent implements OnInit {
  settings: SettingTab[] = [];

  set selected(value: SettingTab) {
    this.store.dispatch(new SetSelectedSettingTab(value));
  }
  get selected(): SettingTab {
    const value = this.store.selectSnapshot(SettingManagementState.getSelectedTab);

    if ((!value || !value.component) && this.settings.length) {
      return this.settings[0];
    }

    return value;
  }

  trackByFn: TrackByFunction<SettingTab> = (_, item) => item.name;

  constructor(private router: Router, private store: Store) {}

  ngOnInit() {
    this.settings = getSettingTabs()
      .filter(setting => this.store.selectSnapshot(ConfigState.getGrantedPolicy(setting.requiredPolicy)))
      .sort((a, b) => a.order - b.order);

    if (!this.selected && this.settings.length) {
      this.selected = this.settings[0];
    }
  }
}
