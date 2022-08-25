import { AbstractControl, ValidatorFn } from '@angular/forms';
import { isNullOrEmpty } from '../utils';

export interface UsernamePatternError {
  usernamePattern: {
    actualValue: string;
  };
}

export interface UsernameOptions {
  pattern?: RegExp;
}

const onlyLetterAndNumberRegex = /^[a-zA-Z0-9]+$/;

export function validateUsername(
  { pattern }: UsernameOptions = { pattern: onlyLetterAndNumberRegex },
): ValidatorFn {
  return (control: AbstractControl): UsernamePatternError | null => {
    const isValid = isValidUserName(control.value, pattern);
    return isValid ? null : { usernamePattern: { actualValue: control.value } };
  };
}

function isValidUserName(value: any, pattern: RegExp): boolean {
  if (isNullOrEmpty(value)) return true;

  return pattern.test(value);
}
