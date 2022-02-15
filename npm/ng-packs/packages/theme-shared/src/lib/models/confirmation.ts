import { LocalizationParam } from '@abp/ng.core';
import {TemplateRef} from "@angular/core";

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
    icon?: string;
    iconTemplate?:string
  }

  export interface DialogData {
    message: LocalizationParam;
    title?: LocalizationParam;
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
