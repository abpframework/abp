import { inject, Injectable } from '@angular/core';
import { ErrorScreenErrorCodes } from '../models/common';
import { HTTP_ERROR_CONFIG } from '../tokens/http-error.token';

@Injectable({
  providedIn: 'root',
})
export class CanCreateCustomErrorService {
  private httpErrorConfig = inject(HTTP_ERROR_CONFIG);

  execute(status: ErrorScreenErrorCodes): boolean {
    return !!(
      this.httpErrorConfig?.errorScreen?.component &&
      this.httpErrorConfig?.errorScreen?.forWhichErrors &&
      this.httpErrorConfig?.errorScreen?.forWhichErrors.indexOf(status) > -1
    );
  }
}
