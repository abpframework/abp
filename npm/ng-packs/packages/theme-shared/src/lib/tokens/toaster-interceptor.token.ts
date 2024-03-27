import { InjectionToken } from '@angular/core';
import { Toaster } from '@abp/ng.theme.shared';

export const HTTP_TOASTER_INTERCEPTOR_CONFIG = new InjectionToken<Toaster.ToasterInterceptorConfig>(
  'HTTP_TOASTER_INTERCEPTOR_CONFIG',
);
