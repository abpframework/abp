import { Config, LocalizationParam } from '@abp/ng.core';

export namespace Confirmation {
  export interface Options {
    id?: any;
    dismissible?: boolean;
    messageLocalizationParams?: string[];
    titleLocalizationParams?: string[];
    hideCancelBtn?: boolean;
    hideYesBtn?: boolean;
    cancelText?: LocalizationParam;
    yesText?: LocalizationParam;
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
