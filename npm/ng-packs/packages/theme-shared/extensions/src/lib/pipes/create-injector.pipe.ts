import { InjectFlags, InjectionToken, Injector, Pipe, PipeTransform, Type } from '@angular/core';
import { InferredData, ToolbarComponent } from '../models/toolbar-actions';
import {
  EXTENSIONS_ACTION_CALLBACK,
  EXTENSIONS_ACTION_DATA,
} from '@abp/ng.theme.shared/extensions';
import { ReadonlyActionData } from '../models/actions';

@Pipe({
  name: 'createInjector',
})
export class CreateInjectorPipe<R> implements PipeTransform {
  constructor(private injector: Injector) {}

  public transform(
    action: ToolbarComponent<R>,
    componentData: ReadonlyActionData<R>,
    getInjectedFn: InferredData<R>['getInjected'],
  ): Injector {
    const get = <T>(token: Type<T> | InjectionToken<T>, notFoundValue?: T, flags?: InjectFlags) => {
      return token === EXTENSIONS_ACTION_DATA
        ? componentData
        : token === EXTENSIONS_ACTION_CALLBACK
        ? (data = componentData) => action.action(data)
        : getInjectedFn.call(this.injector, token, notFoundValue, flags);
    };
    return { get };
  }
}
