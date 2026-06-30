import { AccountService } from '@abp/ng.account.core/proxy';
import { ButtonComponent, getPasswordValidators } from '@abp/ng.theme.shared';
import {
  Component,
  effect,
  Injector,
  inject,
  signal,
  ChangeDetectionStrategy,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
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
  changeDetection: ChangeDetectionStrategy.OnPush,
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
export class ResetPasswordComponent {
  private fb = inject(UntypedFormBuilder);
  private accountService = inject(AccountService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private injector = inject(Injector);

  private readonly queryParams = toSignal(this.route.queryParams);

  form!: UntypedFormGroup;

  readonly inProgress = signal(false);
  readonly isPasswordReset = signal(false);

  mapErrorsFn: Validation.MapErrorsFn = (errors, groupErrors, control) => {
    if (PASSWORD_FIELDS.indexOf(String(control?.name)) < 0) return errors;

    return errors.concat(groupErrors.filter(({ key }) => key === 'passwordMismatch'));
  };

  constructor() {
    effect(() => {
      const params = this.queryParams();
      if (!params) return;

      const { userId, resetToken } = params;
      if (!userId || !resetToken) {
        void this.router.navigateByUrl('/account/login');
        return;
      }

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
    if (this.form.invalid || this.inProgress()) return;

    this.inProgress.set(true);

    this.accountService
      .resetPassword({
        userId: this.form.get('userId')?.value,
        resetToken: this.form.get('resetToken')?.value,
        password: this.form.get('password')?.value,
      })
      .pipe(finalize(() => this.inProgress.set(false)))
      .subscribe(() => {
        this.isPasswordReset.set(true);
      });
  }
}
