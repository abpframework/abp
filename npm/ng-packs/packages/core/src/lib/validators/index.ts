import { Validators } from '@angular/forms';
import { validateMinAge } from './age.validator';
import { validateCreditCard } from './credit-card.validator';
import { validateRange } from './range.validator';
import { validateRequired } from './required.validator';
import { validateStringLength } from './string-length.validator';
import { validateUrl } from './url.validator';
export * from './age.validator';
export * from './credit-card.validator';
export * from './range.validator';
export * from './required.validator';
export * from './string-length.validator';
export * from './url.validator';

export const AbpValidators = {
  creditCard: validateCreditCard,
  emailAddress: () => Validators.email,
  minAge: validateMinAge,
  range: validateRange,
  required: validateRequired,
  stringLength: validateStringLength,
  url: validateUrl,
};
