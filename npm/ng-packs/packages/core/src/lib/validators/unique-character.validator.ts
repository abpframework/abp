import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { isNullOrEmpty } from '../utils';

export interface UniqueCharacterError {
  uniqueCharacter: true;
}

export function validateUniqueCharacter(): ValidatorFn {
  return (control: AbstractControl): UniqueCharacterError | null => {
    if (isNullOrEmpty(control.value)) return null;

    return isUnqiueCharacter(control.value) ? null : { uniqueCharacter: true };
  };
}

function isUnqiueCharacter(value: string): boolean {
  const set = new Set<string>(value.split(''));

  return set.size == value.length;
}
