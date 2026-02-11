import { CustomHttpErrorHandlerService } from '../models';
import {
  CUSTOM_HTTP_ERROR_HANDLER_PRIORITY,
  DEFAULT_ERROR_LOCALIZATIONS,
  DEFAULT_ERROR_MESSAGES,
} from '../constants/default-errors';
import { inject, Injectable } from '@angular/core';
import { CreateErrorComponentService } from './create-error-component.service';

@Injectable({ providedIn: 'root' })
export class UnknownStatusCodeErrorHandlerService implements CustomHttpErrorHandlerService {
  readonly priority = CUSTOM_HTTP_ERROR_HANDLER_PRIORITY.normal;
  private statusText = 'Unknown Error';
  private message = '';
  private createErrorComponentService = inject(CreateErrorComponentService);

  canHandle(error: { status: number; statusText: string; message: string } | undefined): boolean {
    if (error && error.status === 0 && error.statusText === this.statusText) {
      this.message = error.message;
      return true;
    }
    return false;
  }

  execute() {
    this.createErrorComponentService.execute({
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
      },
      details: this.message,
      isHomeShow: false,
    });
  }
}
