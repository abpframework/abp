import { ABP, ForDirective, LocalizationPipe, PermissionDirective } from '@abp/ng.core';
import { SettingTabsService } from '@abp/ng.setting-management/config';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { NgComponentOutlet } from '@angular/common';
import { PageComponent } from '@abp/ng.components/page';
import { Tab, TabContent, TabList, TabPanel, Tabs } from '@angular/aria/tabs';

@Component({
  selector: 'abp-setting-management',
  templateUrl: './setting-management.component.html',
  imports: [
    NgComponentOutlet,
    PageComponent,
    LocalizationPipe,
    PermissionDirective,
    ForDirective,
    Tabs,
    TabList,
    Tab,
    TabPanel,
    TabContent,
  ],
  styles: [
    `
      :host [ngTabPanel][inert] {
        display: none;
      }
    `,
  ],
})
export class SettingManagementComponent implements OnDestroy, OnInit {
  private settingTabsService = inject(SettingTabsService);
  private subscription = new Subscription();

  settings: ABP.Tab[] = [];

  selected!: ABP.Tab;

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
