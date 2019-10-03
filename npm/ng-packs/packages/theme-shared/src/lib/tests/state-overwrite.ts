import { Inject, Injectable, InjectionToken, Provider } from '@angular/core';
import { getActionTypeFromInstance, InitState, NgxsPlugin, NGXS_PLUGINS, setValue } from '@ngxs/store';

export const NGXS_OVERWRITE_PLUGIN_VALUE = new InjectionToken('NGXS_OVERWRITE_PLUGIN_VALUE');

export class StateOverwrite {
  static readonly type = '[StateOverwrite] Patch';
  constructor(public payload: { stateName: string; value: any }) {}
}

@Injectable()
export class OverwritePlugin implements NgxsPlugin {
  initialized: boolean;

  constructor(@Inject(NGXS_OVERWRITE_PLUGIN_VALUE) private options: any) {}

  handle(state, action, next) {
    const type = getActionTypeFromInstance(action);

    if (action instanceof InitState && !this.initialized) {
      state = { ...state, ...this.options };
      console.log(state);
      this.initialized = true;
    }

    if (action instanceof StateOverwrite) {
      state = setValue(state, action.payload.stateName, action.payload.value);
    }

    return next(state, action);
  }
}

export function stateOverwriteProviders(value = {}): Provider[] {
  return [
    {
      provide: NGXS_PLUGINS,
      useClass: OverwritePlugin,
      multi: true,
    },
    {
      provide: NGXS_OVERWRITE_PLUGIN_VALUE,
      useValue: value,
    },
  ];
}
