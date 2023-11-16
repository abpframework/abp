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
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import {EllipsisDirective} from "@abp/ng.theme.shared";

@Component({
  exportAs: 'abpGridActions',
  standalone: true,
  imports: [ CoreModule, NgbDropdownModule, EllipsisDirective],
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

  @Input() readonly index?: number;

  @Input() text = '';

  readonly trackByFn: TrackByFunction<EntityAction<R>> = (_, item) => item.text;

  constructor(injector: Injector) {
    super(injector);
  }
}
