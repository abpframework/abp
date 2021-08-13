import { ABP } from '@abp/ng.core';

export class SetSelectedSettingTab {
  static readonly type = '[SettingManagement] Set Selected Tab';
  constructor(public payload: ABP.Tab) {}
}
