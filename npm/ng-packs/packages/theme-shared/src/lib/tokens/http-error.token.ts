import { InjectionToken } from '@angular/core';
import { CustomHttpErrorHandlerService, HttpErrorConfig, HttpErrorHandler } from '../models/common';

export const HTTP_ERROR_CONFIG = new InjectionToken<HttpErrorConfig>('HTTP_ERROR_CONFIG');

/**
  @deprecated use **`CUSTOM_ERROR_HANDLERS`** injection token instead of this, see more info https://abp.io/docs/latest/framework/ui/angular/http-requests
*/
export const HTTP_ERROR_HANDLER = new InjectionToken<HttpErrorHandler>('HTTP_ERROR_HANDLER');

export const CUSTOM_ERROR_HANDLERS = new InjectionToken<CustomHttpErrorHandlerService[]>(
  'CUSTOM_ERROR_HANDLERS',
);
