import { ABP, SettingTabsService } from '@abp/ng.core';
import { Component, OnDestroy, OnInit, TrackByFunction } from '@angular/core';
import { Store } from '@ngxs/store';
import { Subscription } from 'rxjs';
import { SetSelectedSettingTab } from '../actions/setting-management.actions';
import { SettingManagementState } from '../states/setting-management.state';

@Component({
  selector: 'abp-setting-management',
  templateUrl: './setting-management.component.html',
})
export class SettingManagementComponent implements OnDestroy, OnInit {
  private subscription = new Subscription();
  settings: ABP.Tab[] = [];

  set selected(value: ABP.Tab) {
    this.store.dispatch(new SetSelectedSettingTab(value));
  }
  get selected(): ABP.Tab {
    const value = this.store.selectSnapshot(SettingManagementState.getSelectedTab);

    return value?.component ? value : this.settings[0] || ({} as ABP.Tab);
  }

  trackByFn: TrackByFunction<ABP.Tab> = (_, item) => item.name;

  constructor(private store: Store, private settingTabsService: SettingTabsService) {}

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  ngOnInit() {
    this.subscription.add(
      this.settingTabsService.visible$.subscribe(settings => {
        this.settings = settings;

        if (!this.selected) this.selected = this.settings[0];
      }),
    );
  }
}
