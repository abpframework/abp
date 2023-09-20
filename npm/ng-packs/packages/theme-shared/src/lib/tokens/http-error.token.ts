import { InjectionToken } from '@angular/core';
import { CustomHttpErrorHandlerService, HttpErrorConfig, HttpErrorHandler } from '../models/common';

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

export const CUSTOM_ERROR_HANDLERS = new InjectionToken<CustomHttpErrorHandlerService[]>(
  'CUSTOM_ERROR_HANDLERS',
);
