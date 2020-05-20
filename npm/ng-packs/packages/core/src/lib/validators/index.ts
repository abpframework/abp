import { Validators } from '@angular/forms';
import { validateCreditCard } from './credit-card.validator';
import { validateRange } from './range.validator';
import { validateRequired } from './required.validator';
export * from './credit-card.validator';
export * from './range.validator';
export * from './required.validator';

export const AbpValidators = {
  creditCard: validateCreditCard,
  email: () => Validators.email,
  range: validateRange,
  required: validateRequired,
};
