import { AbstractControl, ValidatorFn } from '@angular/forms';

export interface CreditCardError {
  creditCard: true;
}

export function validateCreditCard(): ValidatorFn {
  return (control: AbstractControl): CreditCardError | null => {
    if (['', null, undefined].indexOf(control.value) > -1) return null;

    return isValidCreditCard(String(control.value)) ? null : { creditCard: true };
  };
}

function isValidCreditCard(value: string): boolean {
  value = value.replace(/[ -]/g, '');

  if (!/^[0-9]{13,19}$/.test(value)) return false;

  let checksum = 0;
  let multiplier = 1;

  for (let i = value.length; i > 0; i--) {
    const digit = Number(value[i - 1]) * multiplier;
    checksum += (digit % 10) + ~~(digit / 10);

    multiplier = (multiplier * 2) % 3;
  }

  return checksum % 10 === 0;
}
