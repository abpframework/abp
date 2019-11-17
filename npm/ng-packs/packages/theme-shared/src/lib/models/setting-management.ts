import { Type } from '@angular/core';

export interface SettingTab {
  component: Type<any>;
  name: string;
  order: number;
  requiredPolicy?: string;
}

const SETTING_TABS = [] as SettingTab[];

export function addSettingTab(tab: SettingTab | SettingTab[]): void {
  if (!Array.isArray(tab)) {
    tab = [tab];
  }

  SETTING_TABS.push(...tab);
}

export function getSettingTabs(): SettingTab[] {
  return SETTING_TABS;
}
