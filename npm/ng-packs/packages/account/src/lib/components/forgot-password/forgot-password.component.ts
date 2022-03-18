import { AccountService } from '@abp/ng.account.core/proxy';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'abp-forgot-password',
  templateUrl: 'forgot-password.component.html',
})
export class ForgotPasswordComponent {
  form: FormGroup;

  inProgress: boolean;

  isEmailSent = false;

  constructor(private fb: FormBuilder, private accountService: AccountService) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.inProgress = true;

    this.accountService
      .sendPasswordResetCode({
        email: this.form.get('email').value,
        appName: 'Angular',
      })
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(() => {
        this.isEmailSent = true;
      });
  }
}
