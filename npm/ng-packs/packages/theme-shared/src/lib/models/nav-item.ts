import { Type } from '@angular/core';

export class NavItem {
  id?: string | number;
  component?: Type<any>;
  html?: string;
  action?: () => void;
  order?: number;
  requiredPolicy?: string;
  visible?: () => boolean;
  constructor(props: Partial<NavItem>) {
    props = { ...props, visible: props.visible || (() => true) };
    Object.assign(this, props);
  }
}
