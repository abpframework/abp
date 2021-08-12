import {
  ChangeDetectionStrategy,
  Component,
  InjectFlags,
  InjectionToken,
  Injector,
  TrackByFunction,
  Type,
} from '@angular/core';
import { ToolbarActionList, ToolbarComponent } from '../../models/toolbar-actions';
import {
  EXTENSIONS_ACTION_CALLBACK,
  EXTENSIONS_ACTION_DATA,
  EXTENSIONS_ACTION_TYPE,
} from '../../tokens/extensions.token';
import { AbstractActionsComponent } from '../abstract-actions/abstract-actions.component';

@Component({
  exportAs: 'abpPageToolbar',
  selector: 'abp-page-toolbar',
  templateUrl: './page-toolbar.component.html',
  providers: [
    {
      provide: EXTENSIONS_ACTION_TYPE,
      useValue: 'toolbarActions',
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageToolbarComponent<R = any> extends AbstractActionsComponent<ToolbarActionList<R>> {
  readonly trackByFn: TrackByFunction<ToolbarComponent<R>> = (_, item) =>
    item.action || item.component;

  constructor(private readonly injector: Injector) {
    super(injector);
  }

  createInjector(action: ToolbarComponent<R>): Injector {
    const get = <T>(token: Type<T> | InjectionToken<T>, notFoundValue?: T, flags?: InjectFlags) => {
      return token === EXTENSIONS_ACTION_DATA
        ? this.data
        : token === EXTENSIONS_ACTION_CALLBACK
        ? (data = this.data) => action.action(data)
        : this.getInjected.call(this.injector, token, notFoundValue, flags);
    };

    return { get };
  }
}
