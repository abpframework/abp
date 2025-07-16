import { ABP, ForDirective, LocalizationPipe, PermissionDirective } from '@abp/ng.core';
import { SettingTabsService } from '@abp/ng.setting-management/config';
import { Component, inject, OnDestroy, OnInit, TrackByFunction } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { PageComponent } from '@abp/ng.components/page';

@Component({
  selector: 'abp-setting-management',
  templateUrl: './setting-management.component.html',
  imports: [CommonModule, PageComponent, LocalizationPipe, PermissionDirective, ForDirective],
})
export class SettingManagementComponent implements OnDestroy, OnInit {
  private settingTabsService = inject(SettingTabsService);
  private subscription = new Subscription();

  settings: ABP.Tab[] = [];

  selected!: ABP.Tab;

  trackByFn: TrackByFunction<ABP.Tab> = (_, item) => item.name;

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
