import { Type } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export interface RootParams {
  httpErrorConfig?: HttpErrorConfig;
  ngbDatepickerOptions?: {
    minDate?: NgbDateStruct;
    maxDate?: NgbDateStruct;
  };
}

export type ErrorScreenErrorCodes = 401 | 403 | 404 | 500;

export interface HttpErrorConfig {
  skipHandledErrorCodes?: ErrorScreenErrorCodes[] | number[];
  errorScreen?: {
    component: Type<any>;
    forWhichErrors?: ErrorScreenErrorCodes[];
    hideCloseIcon?: boolean;
  };
}

export type LocaleDirection = 'ltr' | 'rtl';
