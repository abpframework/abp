import { inject, Injectable } from '@angular/core';
import { AuthService } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { getErrorFromRequestBody } from '../utils/error.utils';
import { CustomHttpErrorHandlerService } from '../models/common';
import { ConfirmationService } from '../services/confirmation.service';
import { CUSTOM_HTTP_ERROR_HANDLER_PRIORITY } from '../constants/default-errors';

@Injectable({ providedIn: 'root' })
export class AbpFormatErrorHandlerService implements CustomHttpErrorHandlerService {
  readonly priority = CUSTOM_HTTP_ERROR_HANDLER_PRIORITY.high;
  private confirmationService = inject(ConfirmationService);
  private authService = inject(AuthService);
  private error: HttpErrorResponse | undefined = undefined;

  private navigateToLogin() {
    return this.authService.navigateToLogin();
  }

  canHandle(error: unknown): boolean {
    if (error instanceof HttpErrorResponse && error.headers.get('_AbpErrorFormat')) {
      this.error = error;
      return true;
    }
    return false;
  }

  execute() {
    const { message, title } = getErrorFromRequestBody(this.error?.error?.error);
    this.confirmationService
      .error(message, title, {
        hideCancelBtn: true,
        yesText: 'AbpAccount::Close',
      })
      .subscribe(() => {
        if (this.error?.status === 401) {
          this.navigateToLogin();
        }
      });
  }
}
