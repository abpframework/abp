import { ConfigGetAppConfiguration, ConfigState } from '@abp/ng.core';
import { Component, Inject, Optional } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from } from 'rxjs';
import { Options } from '../../models/options';

const { maxLength, minLength, required } = Validators;

@Component({
  selector: 'abp-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private oauthService: OAuthService,
    private store: Store,
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

    this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);

    from(
      this.oauthService.fetchTokenUsingPasswordFlow(this.form.get('username').value, this.form.get('password').value),
    ).subscribe({
      next: () => {
        const redirectUrl = window.history.state.redirectUrl || this.options.redirectUrl;

        this.store
          .dispatch(new ConfigGetAppConfiguration())
          .subscribe(() => this.store.dispatch(new Navigate([redirectUrl || '/'])));
      },
      error: () => console.error('an error occured'),
    });
  }
}
