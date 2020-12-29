import { Config, LocalizationParam } from '@abp/ng.core';

export namespace Toaster {
  export interface ToastOptions {
    life?: number;
    sticky?: boolean;
    closable?: boolean;
    tapToDismiss?: boolean;
    messageLocalizationParams?: string[];
    titleLocalizationParams?: string[];
    id: any;
    containerKey?: string;
  }

  export interface Toast {
    message: LocalizationParam;
    title?: LocalizationParam;
    severity?: string;
    options?: ToastOptions;
  }

  export type Severity = 'neutral' | 'success' | 'info' | 'warning' | 'error';
  export type ToasterId = string | number;

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
      message: Config.LocalizationParam,
      title?: Config.LocalizationParam,
      options?: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
    success: (
      message: Config.LocalizationParam,
      title?: Config.LocalizationParam,
      options?: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
    warn: (
      message: Config.LocalizationParam,
      title?: Config.LocalizationParam,
      options?: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
    error: (
      message: Config.LocalizationParam,
      title?: Config.LocalizationParam,
      options?: Partial<Toaster.ToastOptions>,
    ) => ToasterId;
  }
}
