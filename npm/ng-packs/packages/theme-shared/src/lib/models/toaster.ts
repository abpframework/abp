import { Config } from '@abp/ng.core';

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
    message: Config.LocalizationParam;
    title?: Config.LocalizationParam;
    severity?: string;
    options?: ToastOptions;
  }

  export type Severity = 'neutral' | 'success' | 'info' | 'warning' | 'error';

  /**
   * @deprecated Status will be removed from toaster model in v2.2
   */
  export enum Status {
    confirm = 'confirm',
    reject = 'reject',
    dismiss = 'dismiss',
  }
}
