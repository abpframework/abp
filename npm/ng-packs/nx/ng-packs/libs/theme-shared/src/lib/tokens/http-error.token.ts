import { InjectionToken } from '@angular/core';
import { HttpErrorConfig, HttpErrorHandler } from '../models/common';

export function httpErrorConfigFactory(config = {} as HttpErrorConfig) {
  if (config.errorScreen && config.errorScreen.component && !config.errorScreen.forWhichErrors) {
    config.errorScreen.forWhichErrors = [401, 403, 404, 500];
  }

  return {
    skipHandledErrorCodes: [],
    errorScreen: {},
    ...config,
  } as HttpErrorConfig;
}

export const HTTP_ERROR_CONFIG = new InjectionToken<HttpErrorConfig>('HTTP_ERROR_CONFIG');

export const HTTP_ERROR_HANDLER = new InjectionToken<HttpErrorHandler>('HTTP_ERROR_HANDLER');
