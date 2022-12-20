import {
  InjectFlags,
  InjectionToken,
  InjectOptions,
  Injector,
  Pipe,
  PipeTransform,
  Type,
} from '@angular/core';
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
    const get = <T>(
      token: Type<T> | InjectionToken<T>,
      notFoundValue?: T,
      options?: InjectOptions | InjectFlags,
    ) => {
      const componentData = context.getData();
      const componentDataCallback = data => {
        data = data ?? context.getData();
        return action.action(data);
      };
      let extensionData;
      switch (token) {
        case EXTENSIONS_ACTION_DATA:
          extensionData = componentData;
          break;
        case EXTENSIONS_ACTION_CALLBACK:
          extensionData = componentDataCallback;
          break;
        default:
          extensionData = context.getInjected.call(context.injector, token, notFoundValue, options);
      }
      return extensionData;
    };
    return { get };
  }
}
