import { CustomHttpErrorHandlerService } from '../models/common';
import { inject, Injectable } from '@angular/core';
import { AuthService } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { CUSTOM_HTTP_ERROR_HANDLER_PRIORITY } from '../constants/error';

@Injectable({ providedIn: 'root' })
export class TenantResolveErrorHandlerService implements CustomHttpErrorHandlerService {
  priority = CUSTOM_HTTP_ERROR_HANDLER_PRIORITY.high;
  private authService = inject(AuthService);
  private isTenantResolveError(error: unknown) {
    return error instanceof HttpErrorResponse && !!error.headers.get('Abp-Tenant-Resolve-Error');
  }

  canHandle(error: unknown): boolean {
    return this.isTenantResolveError(error);
  }

  execute() {
    this.authService.logout().subscribe();
  }
}
