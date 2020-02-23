import { ChangePassword, ConfigState, ABP } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { comparePasswords, Validation, PasswordRules, validatePassword } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import snq from 'snq';
import { finalize } from 'rxjs/operators';
import { Account } from '../../models/account';

const { minLength, required, maxLength } = Validators;

const PASSWORD_FIELDS = ['newPassword', 'repeatNewPassword'];

@Component({
  selector: 'abp-change-password-form',
  templateUrl: './change-password.component.html',
  exportAs: 'abpChangePasswordForm',
})
export class ChangePasswordComponent
  implements OnInit, Account.ChangePasswordComponentInputs, Account.ChangePasswordComponentOutputs {
  form: FormGroup;

  inProgress: boolean;

  mapErrorsFn: Validation.MapErrorsFn = (errors, groupErrors, control) => {
    if (PASSWORD_FIELDS.indexOf(control.name) < 0) return errors;

    return errors.concat(groupErrors.filter(({ key }) => key === 'passwordMismatch'));
  };

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private toasterService: ToasterService,
  ) {}

  ngOnInit(): void {
    const passwordRules: ABP.Dictionary<string> = this.store.selectSnapshot(
      ConfigState.getSettings('Identity.Password'),
    );
    const passwordRulesArr = [] as PasswordRules;
    let requiredLength = 1;

    if ((passwordRules['Abp.Identity.Password.RequireDigit'] || '').toLowerCase() === 'true') {
      passwordRulesArr.push('number');
    }

    if ((passwordRules['Abp.Identity.Password.RequireLowercase'] || '').toLowerCase() === 'true') {
      passwordRulesArr.push('small');
    }

    if ((passwordRules['Abp.Identity.Password.RequireUppercase'] || '').toLowerCase() === 'true') {
      passwordRulesArr.push('capital');
    }

    if (
      (passwordRules['Abp.Identity.Password.RequireNonAlphanumeric'] || '').toLowerCase() === 'true'
    ) {
      passwordRulesArr.push('special');
    }

    if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
      requiredLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
    }

    this.form = this.fb.group(
      {
        password: ['', required],
        newPassword: [
          '',
          {
            validators: [
              required,
              validatePassword(passwordRulesArr),
              minLength(requiredLength),
              maxLength(128),
            ],
          },
        ],
        repeatNewPassword: [
          '',
          {
            validators: [
              required,
              validatePassword(passwordRulesArr),
              minLength(requiredLength),
              maxLength(128),
            ],
          },
        ],
      },
      {
        validators: [comparePasswords(PASSWORD_FIELDS)],
      },
    );
  }

  onSubmit() {
    if (this.form.invalid) return;
    this.inProgress = true;
    this.store
      .dispatch(
        new ChangePassword({
          currentPassword: this.form.get('password').value,
          newPassword: this.form.get('newPassword').value,
        }),
      )
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe({
        next: () => {
          this.form.reset();
          this.toasterService.success('AbpAccount::PasswordChangedMessage', 'Success', {
            life: 5000,
          });
        },
        error: err => {
          this.toasterService.error(
            snq(() => err.error.error.message, 'AbpAccount::DefaultErrorMessage'),
            'Error',
            {
              life: 7000,
            },
          );
        },
      });
  }
}
