export namespace Toaster {
  export interface Options {
    id?: any;
    closable?: boolean;
    life?: number;
    sticky?: boolean;
    data?: any;
    messageLocalizationParams?: string[];
    titleLocalizationParams?: string[];
  }

  export type Severity = 'success' | 'info' | 'warn' | 'error';

  export const enum Status {
    confirm = 'confirm',
    reject = 'reject',
    dismiss = 'dismiss',
  }
}
