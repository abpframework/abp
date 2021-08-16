import { Directive, Injector, Input } from '@angular/core';
import { ActionData, ActionList } from '../../models/actions';
import { ExtensionsService } from '../../services/extensions.service';
import { EXTENSIONS_ACTION_TYPE, EXTENSIONS_IDENTIFIER } from '../../tokens/extensions.token';

// tslint:disable: directive-class-suffix
// Fix for https://github.com/angular/angular/issues/23904
// @dynamic
@Directive()
export abstract class AbstractActionsComponent<L extends ActionList<any>> extends ActionData<
  InferredRecord<L>
> {
  readonly actionList: L;

  readonly getInjected: InferredData<L>['getInjected'];

  @Input() readonly record: InferredData<L>['record'];

  constructor(injector: Injector) {
    super();

    // tslint:disable-next-line
    this.getInjected = injector.get.bind(injector);
    const extensions = injector.get(ExtensionsService);
    const name = injector.get(EXTENSIONS_IDENTIFIER);
    const type = injector.get(EXTENSIONS_ACTION_TYPE);
    this.actionList = extensions[type].get(name).actions as unknown as L;
  }
}

type InferredData<L> = ActionData<InferredRecord<L>>;
type InferredRecord<L> = L extends ActionList<infer R> ? R : never;
