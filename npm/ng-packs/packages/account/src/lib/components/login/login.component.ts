import { AuthService, SetRemember, ConfigState } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import snq from 'snq';

const { maxLength, minLength, required } = Validators;

@Component({
  selector: 'abp-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  form: FormGroup;

  inProgress: boolean;

  isSelfRegistrationEnabled = true;

  constructor(
    private fb: FormBuilder,
    private oauthService: OAuthService,
    private store: Store,
    private toasterService: ToasterService,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    this.isSelfRegistrationEnabled =
      (
        (this.store.selectSnapshot(
          ConfigState.getSetting('Abp.Account.IsSelfRegistrationEnabled'),
        ) as string) || ''
      ).toLowerCase() !== 'false';

    this.form = this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: ['', [required, maxLength(128)]],
      remember: [false],
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.inProgress = true;
    this.authService
      .login(this.form.get('username').value, this.form.get('password').value)
      .pipe(
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
      .subscribe(() => {
        this.store.dispatch(new SetRemember(this.form.get('remember').value));
      });
  }
}
