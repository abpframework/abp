import { AccountService } from '@abp/ng.account.core/proxy';
import { Component } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { LocalizationPipe } from '@abp/ng.core';
import { ButtonComponent } from '@abp/ng.theme.shared';
import { RouterModule } from '@angular/router';
import { NgxValidateCoreModule } from '@ngx-validate/core';

@Component({
  selector: 'abp-forgot-password',
  templateUrl: 'forgot-password.component.html',
  imports: [
    ReactiveFormsModule,
    RouterModule,
    LocalizationPipe,
    ButtonComponent,
    NgxValidateCoreModule,
  ],
})
export class ForgotPasswordComponent {
  form: UntypedFormGroup;

  inProgress?: boolean;

  isEmailSent = false;

  constructor(
    private fb: UntypedFormBuilder,
    private accountService: AccountService,
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.inProgress = true;

    this.accountService
      .sendPasswordResetCode({
        email: this.form.get('email')?.value,
        appName: 'Angular',
      })
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(() => {
        this.isEmailSent = true;
      });
  }
}
