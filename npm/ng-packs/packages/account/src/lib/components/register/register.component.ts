import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { validatePassword } from '@ngx-validate/core';
import { OAuthService } from 'angular-oauth2-oidc';

const { maxLength, minLength, required, email } = Validators;

@Component({
  selector: 'abp-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  form: FormGroup;

  constructor(private fb: FormBuilder, private oauthService: OAuthService, private router: Router) {
    this.form = this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: [
        '',
        [required, maxLength(32), minLength(6), validatePassword(['small', 'capital', 'number', 'special'])],
      ],
      email: ['', [required, email]],
    });
  }

  onSubmit() {
    if (this.form.invalid) return;
  }
}
