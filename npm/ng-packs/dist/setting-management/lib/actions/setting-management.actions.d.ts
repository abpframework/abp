import { SettingTab } from '@abp/ng.theme.shared';
export declare class SetSelectedSettingTab {
    payload: SettingTab;
    static readonly type = "[SettingManagement] Set Selected Tab";
    constructor(payload: SettingTab);
}
