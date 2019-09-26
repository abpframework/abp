import { Type } from '@angular/core';

export interface SettingTab {
  component: Type<any>;
  name: string;
  order: number;
  requiredPolicy?: string;
}
