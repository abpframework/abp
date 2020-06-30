import {
  ChangeDetectionStrategy,
  Component,
  Injector,
  Input,
  TrackByFunction,
} from '@angular/core';
import { EntityAction, EntityActionList } from '../../models/entity-actions';
import { EXTENSIONS_ACTION_TYPE } from '../../tokens/extensions.token';
import { AbstractActionsComponent } from '../abstract-actions/abstract-actions.component';

@Component({
  exportAs: 'abpGridActions',
  selector: 'abp-grid-actions',
  templateUrl: './grid-actions.component.html',
  providers: [
    {
      provide: EXTENSIONS_ACTION_TYPE,
      useValue: 'entityActions',
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GridActionsComponent<R = any> extends AbstractActionsComponent<EntityActionList<R>> {
  @Input() icon = 'fa fa-cog';

  @Input() readonly index: number;

  @Input() text = '';

  readonly trackByFn: TrackByFunction<EntityAction<R>> = (_, item) => item.text;

  constructor(injector: Injector) {
    super(injector);
  }
}
