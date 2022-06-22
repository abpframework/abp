import { Directive, Injector, Input } from '@angular/core';
import { ActionData, ActionList } from '../../models/actions';
import { ExtensionsService } from '../../services/extensions.service';
import { EXTENSIONS_ACTION_TYPE, EXTENSIONS_IDENTIFIER } from '../../tokens/extensions.token';
import { InferredData, InferredRecord } from '../../models/toolbar-actions';

// Fix for https://github.com/angular/angular/issues/23904
// @dynamic
@Directive()
export abstract class AbstractActionsComponent<L extends ActionList<any>> extends ActionData<
  InferredRecord<L>
> {
  readonly actionList: L;

  readonly getInjected: InferredData<L>['getInjected'];

  @Input() record!: InferredData<L>['record'];

  constructor(injector: Injector) {
    super();

    this.getInjected = injector.get.bind(injector);
    const extensions = injector.get(ExtensionsService);
    const name = injector.get(EXTENSIONS_IDENTIFIER);
    const type = injector.get(EXTENSIONS_ACTION_TYPE);
    this.actionList = extensions[type].get(name).actions as unknown as L;
  }
}
