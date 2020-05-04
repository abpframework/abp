import { AuthService, ConfigState } from '@abp/ng.core';
import { getPasswordValidators, ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { throwError } from 'rxjs';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import snq from 'snq';
import { RegisterRequest } from '../../models';
import { AccountService } from '../../services/account.service';
import { eAccountComponents } from '../../enums/components';
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
    private fb: FormBuilder,
    private accountService: AccountService,
    private oauthService: OAuthService,
    private store: Store,
    private toasterService: ToasterService,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    this.isSelfRegistrationEnabled =
      (
        this.store.selectSnapshot(
          ConfigState.getSetting('Abp.Account.IsSelfRegistrationEnabled'),
        ) || ''
      ).toLowerCase() !== 'false';
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

    this.form = this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: ['', [required, ...getPasswordValidators(this.store)]],
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
    } as RegisterRequest;

    this.accountService
      .register(newUser)
      .pipe(
        switchMap(() => this.authService.login(newUser.userName, newUser.password)),
        catchError(err => {
          this.toasterService.error(
            snq(() => err.error.error_description) ||
              snq(() => err.error.error.message, 'AbpAccount::DefaultErrorMessage'),
            'Error',
            { life: 7000 },
          );
          return throwError(err);
        }),
        finalize(() => (this.inProgress = false)),
      )
      .subscribe();
  }
}
