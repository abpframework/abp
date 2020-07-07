import { Type } from '@angular/core';

export interface NavItem {
  id: string | number;
  component?: Type<any>;
  html?: string;
  action?: () => void;
  order?: number;
  requiredPolicy?: string;
}
