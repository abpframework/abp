import { eLayoutType } from '@abp/ng.core';
import { SettingTab } from '@abp/ng.theme.shared';
import { Component, TrackByFunction } from '@angular/core';
import { Router } from '@angular/router';
import { timer } from 'rxjs';
import { SettingManagementService } from '../services/setting-management.service';

@Component({
  selector: 'abp-setting-layout',
  templateUrl: './setting-layout.component.html',
})
export class SettingLayoutComponent {
  // required for dynamic component
  static type = eLayoutType.setting;

  trackByFn: TrackByFunction<SettingTab> = (_, item) => item.name;

  constructor(public settingManagementService: SettingManagementService, private router: Router) {
    if (
      settingManagementService.selected &&
      this.router.url !== settingManagementService.selected.url &&
      settingManagementService.settings.length
    ) {
      settingManagementService.setSelected(settingManagementService.settings[0]);
    }
  }

  ngOnDestroy() {}
}
