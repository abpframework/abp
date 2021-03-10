import { getPasswordValidators } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { comparePasswords, Validation } from '@ngx-validate/core';
import { finalize } from 'rxjs/operators';
import { AccountService } from '../../proxy/account/account.service';

const PASSWORD_FIELDS = ['password', 'confirmPassword'];

@Component({
  selector: 'abp-reset-password',
  templateUrl: './reset-password.component.html',
})
export class ResetPasswordComponent implements OnInit {
  form: FormGroup;

  inProgress = false;

  isPasswordReset = false;

  mapErrorsFn: Validation.MapErrorsFn = (errors, groupErrors, control) => {
    if (PASSWORD_FIELDS.indexOf(String(control.name)) < 0) return errors;

    return errors.concat(groupErrors.filter(({ key }) => key === 'passwordMismatch'));
  };

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private route: ActivatedRoute,
    private router: Router,
    private injector: Injector,
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(({ userId, resetToken }) => {
      if (!userId || !resetToken) this.router.navigateByUrl('/account/login');

      this.form = this.fb.group(
        {
          userId: [userId, [Validators.required]],
          resetToken: [resetToken, [Validators.required]],
          password: ['', [Validators.required, ...getPasswordValidators(this.injector)]],
          confirmPassword: ['', [Validators.required, ...getPasswordValidators(this.injector)]],
        },
        {
          validators: [comparePasswords(PASSWORD_FIELDS)],
        },
      );
    });
  }

  onSubmit() {
    if (this.form.invalid || this.inProgress) return;

    this.inProgress = true;

    this.accountService
      .resetPassword({
        userId: this.form.get('userId').value,
        resetToken: this.form.get('resetToken').value,
        password: this.form.get('password').value,
      })
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(() => {
        this.isPasswordReset = true;
      });
  }
}
