import { Type } from '@angular/core';

export interface SettingTab {
  name: string;
  order: number;
  component: Type<any>;
  requiredPolicy?: string;
}
