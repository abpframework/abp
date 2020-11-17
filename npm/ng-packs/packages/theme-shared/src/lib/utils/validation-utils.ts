import { ABP, ConfigStateService } from '@abp/ng.core';
import { Injector } from '@angular/core';
import { ValidatorFn, Validators } from '@angular/forms';
import { PasswordRules, validatePassword } from '@ngx-validate/core';

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

  return [validatePassword(passwordRulesArr), minLength(requiredLength), maxLength(128)];
}

function getRuleFn(injector: Injector) {
  const configState = injector.get(ConfigStateService);

  return (key: string) => {
    const passwordRules: ABP.Dictionary<string> = configState.getSettings('Identity.Password');

    return (passwordRules[`Abp.Identity.Password.${key}`] || '').toLowerCase();
  };
}
