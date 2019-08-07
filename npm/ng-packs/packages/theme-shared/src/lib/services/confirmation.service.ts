import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { Confirmation } from '../models/confirmation';

@Injectable({ providedIn: 'root' })
export class ConfirmationService extends AbstractToaster<Confirmation.Options> {
  key: string = 'abpConfirmation';

  sticky: boolean = true;
}
