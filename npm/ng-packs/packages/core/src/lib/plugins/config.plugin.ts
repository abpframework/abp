import { Inject, Injectable, InjectionToken } from '@angular/core';
import {
  actionMatcher,
  InitState,
  NgxsNextPluginFn,
  NgxsPlugin,
  setValue,
  UpdateState,
} from '@ngxs/store';
import { ABP } from '../models/common';

export const NGXS_CONFIG_PLUGIN_OPTIONS = new InjectionToken('NGXS_CONFIG_PLUGIN_OPTIONS');

@Injectable()
export class ConfigPlugin implements NgxsPlugin {
  private initialized = false;

  constructor(@Inject(NGXS_CONFIG_PLUGIN_OPTIONS) private options: ABP.Root) {}

  handle(state: any, event: any, next: NgxsNextPluginFn) {
    const matches = actionMatcher(event);
    const isInitAction = matches(InitState) || matches(UpdateState);

    if (isInitAction && !this.initialized) {
      state = setValue(state, 'ConfigState', {
        ...(state.ConfigState && { ...state.ConfigState }),
        ...this.options,
      });

      this.initialized = true;
    }

    return next(state, event);
  }
}
