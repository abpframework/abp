import { GetAppConfiguration, ConfigState, SessionState } from '@abp/ng.core';
import { Component, Inject, Optional } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, throwError } from 'rxjs';
import { Options } from '../../models/options';
import { ToasterService } from '@abp/ng.theme.shared';
import { catchError, finalize, switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
import { HttpHeaders } from '@angular/common/http';

const { maxLength, minLength, required } = Validators;

@Component({
  selector: 'abp-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  form: FormGroup;

  inProgress: boolean;

  constructor(
    private fb: FormBuilder,
    private oauthService: OAuthService,
    private store: Store,
    private toasterService: ToasterService,
    @Optional() @Inject('ACCOUNT_OPTIONS') private options: Options,
  ) {
    this.oauthService.configure(this.store.selectSnapshot(ConfigState.getOne('environment')).oAuthConfig);
    this.oauthService.loadDiscoveryDocument();

    this.form = this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: ['', [required, maxLength(32)]],
      remember: [false],
    });
  }

  onSubmit() {
    if (this.form.invalid) return;
    // this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);

    this.inProgress = true;
    const tenant = this.store.selectSnapshot(SessionState.getTenant);
    from(
      this.oauthService.fetchTokenUsingPasswordFlow(
        this.form.get('username').value,
        this.form.get('password').value,
        new HttpHeaders({ ...(tenant && tenant.id && { __tenant: tenant.id }) }),
      ),
    )
      .pipe(
        switchMap(() => this.store.dispatch(new GetAppConfiguration())),
        tap(() => {
          const redirectUrl = snq(() => window.history.state).redirectUrl || (this.options || {}).redirectUrl || '/';
          this.store.dispatch(new Navigate([redirectUrl]));
        }),
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
