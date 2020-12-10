import { AbstractControl, ValidatorFn } from '@angular/forms';

export interface MinAgeError {
  minAge: {
    age: number;
  };
}

export interface MinAgeOptions {
  age?: number;
}

export function validateMinAge({ age = 18 }: MinAgeOptions = {}): ValidatorFn {
  return (control: AbstractControl): MinAgeError | null => {
    if (['', null, undefined].indexOf(control.value) > -1) return null;

    return isValidMinAge(control.value, age) ? null : { minAge: { age } };
  };
}

function isValidMinAge(value: string | number, minAge: number) {
  const date = new Date();
  date.setFullYear(date.getFullYear() - minAge);
  date.setHours(23, 59, 59, 999);

  return Number(new Date(value)) <= date.valueOf();
}
