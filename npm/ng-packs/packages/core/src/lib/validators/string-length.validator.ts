import { AbstractControl, ValidatorFn } from '@angular/forms';

export interface StringLengthError {
  maxlength?: number;
  minlength?: number;
}

export interface StringLengthOptions {
  maximumLength?: number;
  minimumLength?: number;
}

export function validateStringLength({
  maximumLength = Infinity,
  minimumLength = 0,
}: StringLengthOptions = {}): ValidatorFn {
  return (control: AbstractControl): StringLengthError | null => {
    if (control.pristine) return null;

    if (!control.value && minimumLength) return { minlength: minimumLength };

    const value = String(control.value);

    return getMinLengthError(value, minimumLength) || getMaxLengthError(value, maximumLength);
  };
}

function getMaxLengthError(value: string, maxlength: number): StringLengthError {
  return value.length > maxlength ? { maxlength } : null;
}

function getMinLengthError(value: string, minlength: number): StringLengthError {
  return value.length < minlength ? { minlength } : null;
}
