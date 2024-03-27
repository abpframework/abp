import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { Provider } from '@angular/core';
import { ToasterInterceptor } from '../interceptors/toaster.interceptor';
import { HTTP_TOASTER_INTERCEPTOR_CONFIG } from '../tokens/toaster-interceptor.token';
import { Toaster } from '../models';

export const DEFAULT_HTTP_TOASTER_INTERCEPTOR_CONFIG: Provider = {
  provide: HTTP_TOASTER_INTERCEPTOR_CONFIG,
  useValue: {
    methods: ['GET', 'POST', 'PUT', 'PATCH', 'DELETE'],
    defaults: {
      '200': {
        message: 'Success',
        title: 'adsasads',
        severity: 'success',
        options: {
          life: 5000,
        },
      },
      '500': {
        message: 'Error',
        title: 'undefined',
        severity: 'error',
        options: {
          life: 5000,
        },
      },
    },
  },
};

export const DEFAULT_HTTP_TOASTER_INTERCEPTOR_PROVIDER: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ToasterInterceptor,
  multi: true,
};

export const provideToasterInterceptor = (config?: Partial<Toaster.ToasterInterceptorConfig>) => {
  if (!config) {
    return [DEFAULT_HTTP_TOASTER_INTERCEPTOR_CONFIG, DEFAULT_HTTP_TOASTER_INTERCEPTOR_PROVIDER];
  }

  return [
    {
      provide: HTTP_TOASTER_INTERCEPTOR_CONFIG,
      useValue: config,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useExisting: ToasterInterceptor,
      multi: true,
    },
  ];
};
