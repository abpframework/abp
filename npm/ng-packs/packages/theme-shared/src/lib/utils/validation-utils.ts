import { Store } from '@ngxs/store';
import { ABP, ConfigState } from '@abp/ng.core';
import { PasswordRules, validatePassword } from '@ngx-validate/core';
import { Validators, ValidatorFn } from '@angular/forms';

const { minLength, maxLength } = Validators;

export function getPasswordValidators(store: Store): ValidatorFn[] {
  const getRule = getRuleFn(store);

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

  return [validatePassword(passwordRulesArr), minLength(requiredLength), maxLength(128)];
}

function getRuleFn(store: Store) {
  return (key: string) => {
    const passwordRules: ABP.Dictionary<string> = store.selectSnapshot(
      ConfigState.getSettings('Identity.Password'),
    );

    return (passwordRules[`Abp.Identity.Password.${key}`] || '').toLowerCase();
  };
}
