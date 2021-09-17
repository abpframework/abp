import { ProfileService } from '@abp/ng.core';
import { getPasswordValidators, ToasterService } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { comparePasswords, Validation } from '@ngx-validate/core';
import { finalize } from 'rxjs/operators';
import { Account } from '../../models/account';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';

const { required } = Validators;

const PASSWORD_FIELDS = ['newPassword', 'repeatNewPassword'];

@Component({
  selector: 'abp-change-password-form',
  templateUrl: './change-password.component.html',
  exportAs: 'abpChangePasswordForm',
})
export class ChangePasswordComponent
  implements OnInit, Account.ChangePasswordComponentInputs, Account.ChangePasswordComponentOutputs
{
  form: FormGroup;

  inProgress: boolean;

  hideCurrentPassword: boolean;

  mapErrorsFn: Validation.MapErrorsFn = (errors, groupErrors, control) => {
    if (PASSWORD_FIELDS.indexOf(String(control.name)) < 0) return errors;

    return errors.concat(groupErrors.filter(({ key }) => key === 'passwordMismatch'));
  };

  constructor(
    private fb: FormBuilder,
    private injector: Injector,
    private toasterService: ToasterService,
    private profileService: ProfileService,
    private manageProfileState: ManageProfileStateService,
  ) {}

  ngOnInit(): void {
    this.hideCurrentPassword = !this.manageProfileState.getProfile()?.hasPassword;

    const passwordValidations = getPasswordValidators(this.injector);

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

    if (this.hideCurrentPassword) this.form.removeControl('password');
  }

  onSubmit() {
    if (this.form.invalid) return;
    this.inProgress = true;
    this.profileService
      .changePassword({
        ...(!this.hideCurrentPassword && { currentPassword: this.form.get('password').value }),
        newPassword: this.form.get('newPassword').value,
      })
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe({
        next: () => {
          this.form.reset();
          this.toasterService.success('AbpAccount::PasswordChangedMessage', '', {
            life: 5000,
          });

          if (this.hideCurrentPassword) {
            this.hideCurrentPassword = false;
            this.form.addControl('password', new FormControl('', [required]));
          }
        },
        error: err => {
          this.toasterService.error(err.error?.error?.message || 'AbpAccount::DefaultErrorMessage');
        },
      });
  }
}
