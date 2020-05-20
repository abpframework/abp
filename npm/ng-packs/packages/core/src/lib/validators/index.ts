import { Validators } from '@angular/forms';
import { validateRequired } from './required.validator';
export * from './required.validator';

export const AbpValidators = {
  email: () => Validators.email,
  required: validateRequired,
};
