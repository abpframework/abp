import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { Tab as NgTab, TabContent, TabList, TabPanel, Tabs } from '@angular/aria/tabs';
import { NgComponentOutlet } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { ABP, ForDirective, LocalizationPipe, PermissionDirective } from '@abp/ng.core';
import { SettingTabsService } from '@abp/ng.setting-management/config';
import { PageComponent } from '@abp/ng.components/page';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
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
    NgTab,
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
export class SettingManagementComponent {
  private settingTabsService = inject(SettingTabsService);

  readonly settings = toSignal(this.settingTabsService.visible$, { initialValue: [] });
  readonly selected = signal<ABP.Tab | undefined>(undefined);

  constructor() {
    effect(() => {
      const settings = this.settings();
      if (!this.selected() && settings.length) {
        this.selected.set(settings[0] as ABP.Tab);
      }
    });
  }
}
