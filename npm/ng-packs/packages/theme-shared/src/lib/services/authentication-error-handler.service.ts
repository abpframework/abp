import { inject, Injectable } from '@angular/core';
import { AuthService, ConfigStateService } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { CustomHttpErrorHandlerService } from '../models/common';
import { CUSTOM_HTTP_ERROR_HANDLER_PRIORITY } from '../constants/default-errors';

@Injectable({ providedIn: 'root' })
export class AbpAuthenticationErrorHandler implements CustomHttpErrorHandlerService {
  readonly priority = CUSTOM_HTTP_ERROR_HANDLER_PRIORITY.veryHigh;
  protected readonly authService = inject(AuthService);
  protected readonly configStateService = inject(ConfigStateService);

  canHandle(error: unknown): boolean {
    return error instanceof HttpErrorResponse && error.status === 401;
  }

  execute() {
    this.configStateService.refreshAppState().subscribe(({ currentUser }) => {
      if (!currentUser.isAuthenticated) {
        this.authService.logout({ noRedirectToLogoutUrl: true });
      }
    });
  }
}
