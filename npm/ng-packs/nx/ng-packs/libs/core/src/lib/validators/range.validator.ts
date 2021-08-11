import { AbstractControl, ValidatorFn } from '@angular/forms';

export interface RangeError {
  range: {
    max: number;
    min: number;
  };
}

export interface RangeOptions {
  maximum?: number;
  minimum?: number;
}

export function validateRange({ maximum = Infinity, minimum = 0 }: RangeOptions = {}): ValidatorFn {
  return (control: AbstractControl): RangeError | null => {
    if (['', null, undefined].indexOf(control.value) > -1) return null;

    const value = Number(control.value);
    return getMinError(value, minimum, maximum) || getMaxError(value, maximum, minimum);
  };
}

function getMaxError(value: number, max: number, min: number): RangeError {
  return value > max ? { range: { max, min } } : null;
}

function getMinError(value: number, min: number, max: number): RangeError {
  return value < min ? { range: { min, max } } : null;
}
