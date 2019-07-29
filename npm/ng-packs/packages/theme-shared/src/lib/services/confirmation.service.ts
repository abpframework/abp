import { Injectable } from '@angular/core';
import { AbstractToasterClass } from '../abstracts/toaster';
import { Confirmation } from '../models/confirmation';

@Injectable({ providedIn: 'root' })
export class ConfirmationService extends AbstractToasterClass<Confirmation.Options> {
  protected key: string = 'abpConfirmation';

  protected sticky: boolean = true;
}
