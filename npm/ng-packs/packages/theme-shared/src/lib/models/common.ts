import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injector, Type } from '@angular/core';
import { Validation } from '@ngx-validate/core';
import { Observable } from 'rxjs';
import { ConfirmationIcons } from '../tokens/confirmation-icons.token';
import { Toaster } from './toaster';

export interface RootParams {
  httpErrorConfig?: HttpErrorConfig;
  validation?: Partial<Validation.Config>;
  confirmationIcons?: Partial<ConfirmationIcons>;
  toastInterceptorConfig?: Partial<Toaster.ToasterInterceptorConfig>;
}

export type ErrorScreenErrorCodes = 0 | 401 | 403 | 404 | 500;

export interface HttpErrorConfig {
  skipHandledErrorCodes?: ErrorScreenErrorCodes[] | number[];
  errorScreen?: {
    component: Type<any>;
    forWhichErrors?: ErrorScreenErrorCodes[];
    hideCloseIcon?: boolean;
  };
}
export type HttpErrorHandler<T = any> = (
  injector: Injector,
  httpError: HttpErrorResponse,
) => Observable<T>;
export type LocaleDirection = 'ltr' | 'rtl';

export interface CustomHttpErrorHandlerService {
  readonly priority: number;
  canHandle(error: unknown): boolean;
  execute(): void;
}

export type HttpResponseEvent = HttpResponse<unknown> | HttpErrorResponse;
