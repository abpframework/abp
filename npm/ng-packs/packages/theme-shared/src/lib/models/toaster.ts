import { LocalizationParam } from '@abp/ng.core';
import { HttpInterceptor } from '@angular/common/http';
import { Type } from '@angular/core';

export namespace Toaster {
  export const SKIP_TOASTER_INTERCEPTOR = 'X-Abp-Skip-Toaster-Interceptor';

  export interface ToastOptions {
    life?: number;
    sticky?: boolean;
    closable?: boolean;
    tapToDismiss?: boolean;
    messageLocalizationParams?: string[];
    titleLocalizationParams?: string[];
    id: any;
    containerKey?: string;
    iconClass?: string;
  }

  export interface Toast {
    message: LocalizationParam;
    title?: LocalizationParam;
    severity?: string;
    options?: ToastOptions;
  }

  export interface ToasterInterceptorConfig {
    methods: HttpMethod[];
    defaults: Partial<ToasterDefaults>;
    customInterceptor: Type<HttpInterceptor>;
    enabled: boolean;
  }

  export type HttpMethod = 'POST' | 'PUT' | 'DELETE' | 'PATCH';
  export type StatusCode = 200 | 201 | 204 | 400 | 401 | 403 | 404 | 500 | 503 | 504 | 0;

  export type Severity = 'neutral' | 'success' | 'info' | 'warning' | 'error';
  export type ToasterId = string | number;

  export type ToasterDefaults = {
    [key in StatusCode]: {
      message: LocalizationParam;
      title: LocalizationParam | undefined;
      severity: Toaster.Severity;
      options: Partial<Toaster.ToastOptions>;
    };
  };
  export interface Service {
    show: (
      message: LocalizationParam,
      title: LocalizationParam,
      severity: Toaster.Severity,
      options: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
    remove: (id: number) => void;
    clear: (containerKey?: string) => void;
    info: (
      message: LocalizationParam,
      title?: LocalizationParam,
      options?: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
    success: (
      message: LocalizationParam,
      title?: LocalizationParam,
      options?: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
    warn: (
      message: LocalizationParam,
      title?: LocalizationParam,
      options?: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
    error: (
      message: LocalizationParam,
      title?: LocalizationParam,
      options?: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
  }
}
