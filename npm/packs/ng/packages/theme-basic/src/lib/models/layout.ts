import { TemplateRef } from '@angular/core';

export namespace Layout {
  export interface State {
    navigationElements: NavigationElement[];
  }

  export interface NavigationElement {
    name: string;
    element: TemplateRef<any>;
    order?: number;
  }
}
