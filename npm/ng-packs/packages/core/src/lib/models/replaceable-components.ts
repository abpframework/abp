import { Type } from '@angular/core';

export namespace ReplaceableComponents {
  export interface State {
    replaceableComponents: ReplaceableComponent[];
  }

  export interface ReplaceableComponent {
    component: Type<any>;
    key: string;
  }
}
