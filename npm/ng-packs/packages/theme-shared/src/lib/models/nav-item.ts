import { Type } from '@angular/core';
import { Observable, of } from 'rxjs';

export class NavItem {
  id?: string | number;
  component?: Type<any>;
  html?: string;
  action?: () => void;
  order?: number;
  requiredPolicy?: string;
  visible?: () => boolean;
  visible$?: Observable<boolean>;
  constructor(props: Partial<NavItem>) {
    props = {
      ...props,
      visible: props.visible || (() => true),
      visible$: props.visible$ || of(true),
    };
    Object.assign(this, props);
  }
}
