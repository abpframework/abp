import { InjectionToken } from '@angular/core';
import { HttpErrorConfig } from '../models/common';

export function httpErrorConfigFactory(config = {} as HttpErrorConfig) {
  if (config.errorScreen && config.errorScreen.component && !config.errorScreen.forWhichErrors) {
    config.errorScreen.forWhichErrors = [401, 403, 404, 500];
  }

  return {
    errorScreen: {},
    ...config,
  } as HttpErrorConfig;
}

export const HTTP_ERROR_CONFIG = new InjectionToken('HTTP_ERROR_CONFIG');
