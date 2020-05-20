import { Validators } from '@angular/forms';
import { validateCreditCard } from './credit-card.validator';
import { validateRequired } from './required.validator';
export * from './credit-card.validator';
export * from './required.validator';

export const AbpValidators = {
  creditCard: validateCreditCard,
  email: () => Validators.email,
  required: validateRequired,
};
