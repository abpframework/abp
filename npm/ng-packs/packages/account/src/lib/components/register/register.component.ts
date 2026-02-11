import { AccountService, RegisterDto } from '@abp/ng.account.core/proxy';
import {
  AuthService,
  AutofocusDirective,
  ConfigStateService,
  LocalizationPipe,
} from '@abp/ng.core';
import { ButtonComponent, getPasswordValidators, ToasterService } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit, inject } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { throwError } from 'rxjs';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import { eAccountComponents } from '../../enums/components';
import { getRedirectUrl } from '../../utils/auth-utils';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { RouterLink } from '@angular/router';

const { maxLength, required, email } = Validators;

@Component({
  selector: 'abp-register',
  templateUrl: './register.component.html',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    NgxValidateCoreModule,
    LocalizationPipe,
    ButtonComponent,
    AutofocusDirective,
  ],
})
export class RegisterComponent implements OnInit {
  protected fb = inject(UntypedFormBuilder);
  protected accountService = inject(AccountService);
  protected configState = inject(ConfigStateService);
  protected toasterService = inject(ToasterService);
  protected authService = inject(AuthService);
  protected injector = inject(Injector);

  form!: UntypedFormGroup;

  inProgress?: boolean;

  isSelfRegistrationEnabled = true;

  authWrapperKey = eAccountComponents.AuthWrapper;

  ngOnInit() {
    this.init();
    this.buildForm();
  }

  protected init() {
    this.isSelfRegistrationEnabled =
      (this.configState.getSetting('Abp.Account.IsSelfRegistrationEnabled') || '').toLowerCase() !==
      'false';

    if (!this.isSelfRegistrationEnabled) {
      this.toasterService.warn(
        {
          key: 'AbpAccount::SelfRegistrationDisabledMessage',
          defaultValue: 'Self registration is disabled.',
        },
        '',
        { life: 10000 },
      );
      return;
    }
  }

  protected buildForm() {
    this.form = this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: ['', [required, ...getPasswordValidators(this.injector)]],
      email: ['', [required, email]],
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.inProgress = true;

    const newUser = {
      userName: this.form.get('username')?.value,
      password: this.form.get('password')?.value,
      emailAddress: this.form.get('email')?.value,
      appName: 'Angular',
    } as RegisterDto;

    this.accountService
      .register(newUser)
      .pipe(
        switchMap(() =>
          this.authService.login({
            username: newUser.userName,
            password: newUser.password,
            redirectUrl: getRedirectUrl(this.injector),
          }),
        ),
        catchError(err => {
          this.toasterService.error(
            err.error?.error_description ||
              err.error?.error.message ||
              'AbpAccount::DefaultErrorMessage',
            '',
            { life: 7000 },
          );

          return throwError(err);
        }),
        finalize(() => (this.inProgress = false)),
      )
      .subscribe();
  }
}
