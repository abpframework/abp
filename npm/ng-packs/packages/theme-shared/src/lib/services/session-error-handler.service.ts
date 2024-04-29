import { inject, Injectable } from '@angular/core';
import { AuthService } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { getErrorFromRequestBody } from '../utils/error.utils';
import { CustomHttpErrorHandlerService } from '../models/common';
import { ConfirmationService } from '../services/confirmation.service';
import { CUSTOM_HTTP_ERROR_HANDLER_PRIORITY } from '../constants/default-errors';

@Injectable({ providedIn: 'root' })
export class AbpSessionErrorHandlerService implements CustomHttpErrorHandlerService {
  readonly priority = CUSTOM_HTTP_ERROR_HANDLER_PRIORITY.high;
  private authService = inject(AuthService);

  canHandle(error: unknown): boolean {
    console.log(error);
    debugger;
    if (
      error instanceof HttpErrorResponse &&
      error.headers.get('expires') &&
      error.headers.get('pragma')
    ) {
      return true;
    }
    return false;
  }

  execute() {
    this.authService.logout();
  }
}
