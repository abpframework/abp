import { InjectFlags, InjectionToken, Injector, Pipe, PipeTransform, Type } from '@angular/core';
import { HasCreateInjectorPipe, ToolbarComponent } from '../models/toolbar-actions';
import { EXTENSIONS_ACTION_CALLBACK, EXTENSIONS_ACTION_DATA } from '../tokens/extensions.token';

@Pipe({
  name: 'createInjector',
})
export class CreateInjectorPipe<R> implements PipeTransform {
  public transform(
    _: any,
    action: ToolbarComponent<R>,
    context: HasCreateInjectorPipe<R>,
  ): Injector {
    const get = <T>(token: Type<T> | InjectionToken<T>, notFoundValue?: T, flags?: InjectFlags) => {
      const componentData = context.getData();
      return token === EXTENSIONS_ACTION_DATA
        ? componentData
        : token === EXTENSIONS_ACTION_CALLBACK
        ? data => {
            data = data ?? context.getData();
            return action.action(data);
          }
        : context.getInjected.call(context.injector, token, notFoundValue, flags);
    };
    return { get };
  }
}
