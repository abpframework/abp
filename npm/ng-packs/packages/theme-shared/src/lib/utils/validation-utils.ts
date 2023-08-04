import { ABP, ConfigStateService } from '@abp/ng.core';
import { Injector } from '@angular/core';
import { AbstractControl, ValidatorFn, Validators } from '@angular/forms';
import { normalizeDiacritics, PasswordRules } from '@ngx-validate/core';
import { PasswordRule } from '../models/validation';

const { minLength, maxLength } = Validators;

export function getPasswordValidators(injector: Injector): ValidatorFn[] {
  const getRule = getRuleFn(injector);

  const passwordRulesArr = [] as PasswordRules;
  let requiredLength = 1;

  if (getRule('RequireDigit') === 'true') {
    passwordRulesArr.push('number');
  }

  if (getRule('RequireLowercase') === 'true') {
    passwordRulesArr.push('small');
  }

  if (getRule('RequireUppercase') === 'true') {
    passwordRulesArr.push('capital');
  }

  if (getRule('RequireNonAlphanumeric') === 'true') {
    passwordRulesArr.push('special');
  }

  if (Number.isInteger(+getRule('RequiredLength'))) {
    requiredLength = +getRule('RequiredLength');
  }

  const passwordValidators = passwordRulesArr.map(rule => validatePassword(rule));
  return [...passwordValidators, minLength(requiredLength), maxLength(128)];
}

function getRuleFn(injector: Injector) {
  const configState = injector.get(ConfigStateService);

  return (key: string) => {
    const passwordRules: ABP.Dictionary<string> = configState.getSettings('Identity.Password');

    return (passwordRules[`Abp.Identity.Password.${key}`] || '').toLowerCase();
  };
}
const errorMessageMap = {
  small: 'passwordRequiresLower',
  capital: 'passwordRequiresUpper',
  number: 'passwordRequiresDigit',
  special: 'passwordRequiresNonAlphanumeric',
};

export function validatePassword(shouldContain: PasswordRule): ValidatorFn {
  return (control: AbstractControl) => {
    if (!control.value) return null;

    const value = normalizeDiacritics(control.value);

    const regexMap = {
      small: /.*[a-z].*/,
      capital: /.*[A-Z].*/,
      number: /.*[0-9].*/,
      special: /.*[^0-9a-zA-Z].*/,
    };
    const regex = regexMap[shouldContain];

    const isValid = regex.test(value);

    if (isValid) {
      return null;
    }

    const error = errorMessageMap[shouldContain];

    return {
      [error]: true,
    };
  };
}
