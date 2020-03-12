import { Config } from '@abp/ng.core';

export namespace Confirmation {
  export interface Options {
    id?: any;
    closable?: boolean;
    messageLocalizationParams?: string[];
    titleLocalizationParams?: string[];
    hideCancelBtn?: boolean;
    hideYesBtn?: boolean;
    cancelText?: Config.LocalizationParam;
    yesText?: Config.LocalizationParam;
  }

  export interface DialogData {
    message: Config.LocalizationParam;
    title?: Config.LocalizationParam;
    severity?: Severity;
    options?: Partial<Options>;
  }

  export type Severity = 'neutral' | 'success' | 'info' | 'warning' | 'error';

  export enum Status {
    confirm = 'confirm',
    reject = 'reject',
    dismiss = 'dismiss',
  }
}
