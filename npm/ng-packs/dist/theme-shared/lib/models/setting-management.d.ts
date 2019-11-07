import { Type } from '@angular/core';
export interface SettingTab {
    component: Type<any>;
    name: string;
    order: number;
    requiredPolicy?: string;
}
export declare function addSettingTab(tab: SettingTab | SettingTab[]): void;
export declare function getSettingTabs(): SettingTab[];
