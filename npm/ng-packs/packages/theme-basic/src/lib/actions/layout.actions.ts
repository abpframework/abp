import { Layout } from '../models/layout';

export class LayoutAddNavigationElement {
  static readonly type = '[Layout] Add Navigation Element';
  constructor(public payload: Layout.NavigationElement | Layout.NavigationElement[]) {}
}

export class LayoutRemoveNavigationElementByName {
  static readonly type = '[Layout] Remove Navigation ElementByName';
  constructor(public name: string) {}
}
