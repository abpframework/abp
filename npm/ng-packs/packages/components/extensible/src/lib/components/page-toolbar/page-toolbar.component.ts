import { ChangeDetectionStrategy, Component, Injector, TrackByFunction } from '@angular/core';
import {
  HasCreateInjectorPipe,
  ToolbarAction,
  ToolbarActionDefault,
  ToolbarActionList,
  ToolbarComponent,
} from '../../models/toolbar-actions';
import { EXTENSIONS_ACTION_TYPE } from '../../tokens/extensions.token';
import { AbstractActionsComponent } from '../abstract-actions/abstract-actions.component';
import { CreateInjectorPipe } from '../../pipes/create-injector.pipe';
import { LocalizationModule, PermissionDirective } from '@abp/ng.core';
import { NgClass, NgComponentOutlet } from '@angular/common';

@Component({
  exportAs: 'abpPageToolbar',
  selector: 'abp-page-toolbar',
  standalone: true,
  imports: [
    CreateInjectorPipe,
    PermissionDirective,
    LocalizationModule,
    NgClass,
    NgComponentOutlet,
  ],
  templateUrl: './page-toolbar.component.html',
  providers: [
    {
      provide: EXTENSIONS_ACTION_TYPE,
      useValue: 'toolbarActions',
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageToolbarComponent<R = any>
  extends AbstractActionsComponent<ToolbarActionList<R>>
  implements HasCreateInjectorPipe<R>
{
  defaultBtnClass = 'btn btn-sm btn-primary';

  getData = () => this.data;

  readonly trackByFn: TrackByFunction<ToolbarComponent<R>> = (_, item) =>
    item.action || item.component;

  constructor(public readonly injector: Injector) {
    super(injector);
  }

  asToolbarAction(value: ToolbarActionDefault): { value: ToolbarAction } {
    return {
      value: value as ToolbarAction,
    };
  }
}
