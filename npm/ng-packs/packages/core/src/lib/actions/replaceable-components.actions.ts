import { ReplaceableComponents } from '../models/replaceable-components';

export class AddReplaceableComponent {
  static readonly type = '[ReplaceableComponents] Add';
  constructor(public payload: ReplaceableComponents.ReplaceableComponent) {}
}
