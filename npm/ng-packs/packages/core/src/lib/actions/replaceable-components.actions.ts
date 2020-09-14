import { ReplaceableComponents } from '../models/replaceable-components';

// tslint:disable: max-line-length
/**
 * @deprecated To be deleted in v4.0. Use ReplaceableComponentsService instead. See the doc (https://docs.abp.io/en/abp/latest/UI/Angular/Component-Replacement)
 */
export class AddReplaceableComponent {
  static readonly type = '[ReplaceableComponents] Add';
  constructor(
    public payload: ReplaceableComponents.ReplaceableComponent,
    public reload?: boolean,
  ) {}
}
