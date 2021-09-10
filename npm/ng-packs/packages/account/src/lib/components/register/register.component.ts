import { AuthService, ConfigStateService } from '@abp/ng.core';
import { getPasswordValidators, ToasterService } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { throwError } from 'rxjs';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import { eAccountComponents } from '../../enums/components';
import { AccountService } from '../../proxy/account/account.service';
import { RegisterDto } from '../../proxy/account/models';
import { getRedirectUrl } from '../../utils/auth-utils';
const { maxLength, required, email } = Validators;

@Component({
  selector: 'abp-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent implements OnInit {
  form: FormGroup;

  inProgress: boolean;

  isSelfRegistrationEnabled = true;

  authWrapperKey = eAccountComponents.AuthWrapper;

  constructor(
    protected fb: FormBuilder,
    protected accountService: AccountService,
    protected configState: ConfigStateService,
    protected toasterService: ToasterService,
    protected authService: AuthService,
    protected injector: Injector,
  ) {}

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
        null,
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
      userName: this.form.get('username').value,
      password: this.form.get('password').value,
      emailAddress: this.form.get('email').value,
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
            null,
            { life: 7000 },
          );

          return throwError(err);
        }),
        finalize(() => (this.inProgress = false)),
      )
      .subscribe();
  }
}
