import { InjectionToken } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

export const TENANT_NOT_FOUND_BY_NAME = new InjectionToken<
  (HttpErrorResponse: HttpErrorResponse) => void
>('TENANT_NOT_FOUND_BY_NAME');
