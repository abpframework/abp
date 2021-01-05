import { Type } from '@angular/core';
import { Validation } from '@ngx-validate/core';

export interface RootParams {
  httpErrorConfig: HttpErrorConfig;
  validation?: Partial<Validation.Config>;
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
