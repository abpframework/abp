import { Type } from '@angular/core';

export namespace ReplaceableComponents {
  export interface State {
    replaceableComponents: Data[];
  }

  export interface Data {
    component: Type<any>;
    key: string;
  }
}
