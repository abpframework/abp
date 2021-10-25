import { ABP } from '@abp/ng.core';
import { Component, OnDestroy, OnInit, TrackByFunction } from '@angular/core';
import { SettingTabsService } from '@abp/ng.setting-management/config';
import { Subscription } from 'rxjs';

@Component({
  selector: 'abp-setting-management',
  templateUrl: './setting-management.component.html',
})
export class SettingManagementComponent implements OnDestroy, OnInit {
  private subscription = new Subscription();
  settings: ABP.Tab[] = [];

  selected: ABP.Tab;

  trackByFn: TrackByFunction<ABP.Tab> = (_, item) => item.name;

  constructor(private settingTabsService: SettingTabsService) {}

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
