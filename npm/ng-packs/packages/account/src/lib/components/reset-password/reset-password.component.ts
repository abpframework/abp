import { AccountService } from '@abp/ng.account.core/proxy';
import { ButtonComponent, getPasswordValidators } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit, inject } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { comparePasswords, NgxValidateCoreModule, Validation } from '@ngx-validate/core';
import { finalize } from 'rxjs/operators';
import { LocalizationPipe } from '@abp/ng.core';

const PASSWORD_FIELDS = ['password', 'confirmPassword'];

@Component({
  selector: 'abp-reset-password',
  templateUrl: './reset-password.component.html',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    NgxValidateCoreModule,
    LocalizationPipe,
    ButtonComponent,
  ],
})
export class ResetPasswordComponent implements OnInit {
  private fb = inject(UntypedFormBuilder);
  private accountService = inject(AccountService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private injector = inject(Injector);

  form!: UntypedFormGroup;

  inProgress = false;

  isPasswordReset = false;

  mapErrorsFn: Validation.MapErrorsFn = (errors, groupErrors, control) => {
    if (PASSWORD_FIELDS.indexOf(String(control?.name)) < 0) return errors;

    return errors.concat(groupErrors.filter(({ key }) => key === 'passwordMismatch'));
  };

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
        userId: this.form.get('userId')?.value,
        resetToken: this.form.get('resetToken')?.value,
        password: this.form.get('password')?.value,
      })
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(() => {
        this.isPasswordReset = true;
      });
  }
}
