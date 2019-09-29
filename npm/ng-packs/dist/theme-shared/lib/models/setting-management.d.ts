export interface SettingTab {
    name: string;
    order: number;
    requiredPolicy?: string;
    url?: string;
}
