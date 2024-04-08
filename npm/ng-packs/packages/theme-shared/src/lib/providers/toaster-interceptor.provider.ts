import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { Provider } from '@angular/core';
import { ToasterInterceptor } from '../interceptors/toaster.interceptor';
import { HTTP_TOASTER_INTERCEPTOR_CONFIG } from '../tokens/toaster-interceptor.token';
import { Toaster } from '../models';

const methods = ['POST', 'PUT', 'PATCH', 'DELETE'];
const defaults: Partial<Toaster.ToasterDefaults> = {
  '200': {
    message: 'AbpUi::SavedSuccessfully',
    title: 'AbpUi::Saved',
    severity: 'success',
    options: {
      life: 5000,
      titleLocalizationParams: ['AbpUi::Saved'],
      messageLocalizationParams: ['AbpUi::SavedSuccessfully'],
    },
  },
  '201': {
    message: 'AbpUi::CreatedSuccessfully',
    title: 'AbpUi::Created',
    severity: 'success',
    options: {
      life: 5000,
      titleLocalizationParams: ['AbpUi::Created'],
      messageLocalizationParams: ['AbpUi::CreatedSuccessfully'],
    },
  },
  '204': {
    message: 'AbpUi::CompletedSuccessfully',
    title: 'AbpUi::Completed',
    severity: 'info',
    options: {
      life: 5000,
      titleLocalizationParams: ['AbpUi::Completed'],
      messageLocalizationParams: ['AbpUi::CompletedSuccessfully'],
    },
  },
  '400': {
    message: 'AbpUi::BadRequest',
    title: 'AbpUi::BadRequest',
    severity: 'error',
    options: {
      life: 5000,
      titleLocalizationParams: ['AbpUi::BadRequest'],
      messageLocalizationParams: ['AbpUi::BadRequest'],
    },
  },
  '401': {
    message: 'AbpUi::Unauthorized',
    title: 'AbpUi::Unauthorized',
    severity: 'error',
    options: {
      life: 5000,
      titleLocalizationParams: ['AbpUi::Unauthorized'],
      messageLocalizationParams: ['AbpUi::Unauthorized'],
    },
  },
  '403': {
    message: 'AbpUi::Forbidden',
    title: 'AbpUi::Forbidden',
    severity: 'error',
    options: {
      life: 5000,
      titleLocalizationParams: ['AbpUi::Forbidden'],
      messageLocalizationParams: ['AbpUi::Forbidden'],
    },
  },
  '500': {
    message: 'AbpUi::InternalServerError',
    title: 'AbpUi::Error',
    severity: 'error',
    options: {
      life: 5000,
      titleLocalizationParams: ['AbpUi::Error'],
      messageLocalizationParams: ['AbpUi::InternalServerError'],
    },
  },
};

export const DEFAULT_HTTP_TOASTER_INTERCEPTOR_CONFIG: Provider = {
  provide: HTTP_TOASTER_INTERCEPTOR_CONFIG,
  useValue: {
    methods,
    defaults,
  },
};

export const DEFAULT_HTTP_TOASTER_INTERCEPTOR_PROVIDER: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ToasterInterceptor,
  multi: true,
};

export const provideToasterInterceptor = (config?: Partial<Toaster.ToasterInterceptorConfig>) => {
  if (!config || (!config?.enabled && !config?.customInterceptor)) {
    return [];
  }

  if (config.enabled && !config.defaults && !config.methods) {
    console.warn(
      'ToasterInterceptor is enabled but "defaults" and "methods" are not provided. Using default configuration.',
    );

    return [DEFAULT_HTTP_TOASTER_INTERCEPTOR_CONFIG, DEFAULT_HTTP_TOASTER_INTERCEPTOR_PROVIDER];
  }

  return [
    {
      provide: HTTP_TOASTER_INTERCEPTOR_CONFIG,
      useValue: config,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: config?.customInterceptor || ToasterInterceptor,
      multi: true,
    },
  ];
};
