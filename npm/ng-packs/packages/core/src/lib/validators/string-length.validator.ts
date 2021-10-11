import { AbstractControl, ValidatorFn } from '@angular/forms';

export interface StringLengthError {
  maxlength?: {
    requiredLength: number;
  };
  minlength?: {
    requiredLength: number;
  };
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
    if (['', null, undefined].indexOf(control.value) > -1) return null;

    const value = String(control.value);

    return getMinLengthError(value, minimumLength) || getMaxLengthError(value, maximumLength);
  };
}

function getMaxLengthError(value: string, requiredLength: number): StringLengthError {
  return value.length > requiredLength ? { maxlength: { requiredLength } } : null;
}

function getMinLengthError(value: string, requiredLength: number): StringLengthError {
  return value.length < requiredLength ? { minlength: { requiredLength } } : null;
}
