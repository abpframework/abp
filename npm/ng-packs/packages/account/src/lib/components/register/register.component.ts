import { ConfigState, GetAppConfiguration, ABP, SessionState } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, throwError } from 'rxjs';
import { catchError, finalize, switchMap, take, tap } from 'rxjs/operators';
import snq from 'snq';
import { RegisterRequest } from '../../models';
import { AccountService } from '../../services/account.service';
import { PasswordRules, validatePassword } from '@ngx-validate/core';
import { HttpHeaders } from '@angular/common/http';
const { maxLength, minLength, required, email } = Validators;

@Component({
  selector: 'abp-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent implements OnInit {
  form: FormGroup;

  inProgress: boolean;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private oauthService: OAuthService,
    private store: Store,
    private toasterService: ToasterService,
  ) {
    this.oauthService.configure(
      this.store.selectSnapshot(ConfigState.getOne('environment')).oAuthConfig,
    );
    this.oauthService.loadDiscoveryDocument();
  }

  ngOnInit() {
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

    if (+(passwordRules['Abp.Identity.Password.RequiredUniqueChars'] || 0) > 0) {
      passwordRulesArr.push('special');
    }

    if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
      requiredLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
    }

    this.form = this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: [
        '',
        [required, validatePassword(passwordRulesArr), minLength(requiredLength), maxLength(32)],
      ],
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

    const tenant = this.store.selectSnapshot(SessionState.getTenant);

    this.accountService
      .register(newUser)
      .pipe(
        switchMap(() =>
          from(
            this.oauthService.fetchTokenUsingPasswordFlow(
              newUser.userName,
              newUser.password,
              new HttpHeaders({
                ...(tenant && tenant.id && { __tenant: tenant.id }),
              }),
            ),
          ),
        ),
        switchMap(() => this.store.dispatch(new GetAppConfiguration())),
        tap(() => this.store.dispatch(new Navigate(['/']))),
        take(1),
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
