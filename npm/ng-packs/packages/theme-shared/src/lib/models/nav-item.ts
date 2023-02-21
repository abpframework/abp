import { Injector, Type } from '@angular/core';
import { Observable, of } from 'rxjs';

export class NavItem {
  id?: string | number;
  component?: Type<any>;
  html?: string;
  action?: () => void;
  order?: number;
  requiredPolicy?: string;
  visible?: NavBarPropPredicate<NavItem>;
  constructor(props: Partial<NavItem>) {
    Object.assign(this, props);
  }
}

export type NavBarPropPredicate<T> = (
  prop?: T,
  injector?: Injector,
) => boolean | Promise<boolean> | Observable<boolean>;
