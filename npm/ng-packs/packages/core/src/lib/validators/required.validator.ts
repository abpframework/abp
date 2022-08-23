import { AbstractControl, ValidatorFn } from '@angular/forms';

export interface RequiredError {
  required: true;
}

export interface RequiredOptions {
  allowEmptyStrings?: boolean;
}

export function validateRequired({ allowEmptyStrings }: RequiredOptions = {}): ValidatorFn {
  const required = (control: AbstractControl): RequiredError | null => {
    return isValidRequired(control.value, allowEmptyStrings) ? null : { required: true };
  };
  return required;
}

function isValidRequired(value: any, allowEmptyStrings: boolean): boolean {
  if (value || value === 0 || value === false) return true;

  if (allowEmptyStrings && value === '') return true;

  return false;
}
