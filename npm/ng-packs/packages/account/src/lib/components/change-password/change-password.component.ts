import { ChangePassword } from '@abp/ng.core';
import { getPasswordValidators, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { comparePasswords, Validation } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import { finalize } from 'rxjs/operators';
import snq from 'snq';
import { Account } from '../../models/account';

const { required } = Validators;

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
    if (PASSWORD_FIELDS.indexOf(String(control.name)) < 0) return errors;

    return errors.concat(groupErrors.filter(({ key }) => key === 'passwordMismatch'));
  };

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private toasterService: ToasterService,
  ) {}

  ngOnInit(): void {
    const passwordValidations = getPasswordValidators(this.store);

    this.form = this.fb.group(
      {
        password: ['', required],
        newPassword: [
          '',
          {
            validators: [required, ...passwordValidations],
          },
        ],
        repeatNewPassword: [
          '',
          {
            validators: [required, ...passwordValidations],
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
