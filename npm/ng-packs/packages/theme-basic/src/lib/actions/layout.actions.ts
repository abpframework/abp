import { Layout } from '../models/layout';

export class LayoutAddNavigationElement {
  static readonly type = '[Layout] Add Navigation Element';
  constructor(public payload: Layout.NavigationElement | Layout.NavigationElement[]) {}
}
