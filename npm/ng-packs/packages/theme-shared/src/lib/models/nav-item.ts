import { Injector, Type } from '@angular/core';
import { Observable } from 'rxjs';

export interface Badge {
  count?: number | Observable<number>;
  color?: string;
  icon?: string;
}

export class NavItem {
  id?: string | number;
  name?: string;
  description?: string;
  badge?: Badge;
  component?: Type<any>;
  html?: string;
  action?: () => void;
  order?: number;
  requiredPolicy?: string;
  visible?: NavBarPropPredicate<NavItem>;
  icon?: string;
  constructor(props: Partial<NavItem>) {
    Object.assign(this, props);
  }
}

export type NavBarPropPredicate<T> = (
  prop?: T,
  injector?: Injector,
) => boolean | Promise<boolean> | Observable<boolean>;
