import {
  ChangeDetectionStrategy,
  Component,
  TrackByFunction,
  input
} from '@angular/core';
import { EntityAction, EntityActionList } from '../../models/entity-actions';
import { EXTENSIONS_ACTION_TYPE } from '../../tokens/extensions.token';
import { AbstractActionsComponent } from '../abstract-actions/abstract-actions.component';
import { NgbDropdownModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationPipe, PermissionDirective } from '@abp/ng.core';
import { EllipsisDirective } from '@abp/ng.theme.shared';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  exportAs: 'abpGridActions',
  imports: [
    NgbDropdownModule,
    EllipsisDirective,
    PermissionDirective,
    LocalizationPipe,
    NgTemplateOutlet,
    NgbTooltipModule,
  ],
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
  readonly icon = input('fa fa-cog');
  readonly index = input<number | undefined>(undefined);
  readonly text = input('');

  readonly trackByFn: TrackByFunction<EntityAction<R>> = (_, item) => item.text;

  constructor() {
    super();
  }
}
