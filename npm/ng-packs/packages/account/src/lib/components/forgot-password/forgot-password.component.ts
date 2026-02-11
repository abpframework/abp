import { AccountService } from '@abp/ng.account.core/proxy';
import { Component, inject } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { LocalizationPipe } from '@abp/ng.core';
import { ButtonComponent } from '@abp/ng.theme.shared';
import { RouterLink } from '@angular/router';
import { NgxValidateCoreModule } from '@ngx-validate/core';

@Component({
  selector: 'abp-forgot-password',
  templateUrl: 'forgot-password.component.html',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    LocalizationPipe,
    ButtonComponent,
    NgxValidateCoreModule,
  ],
})
export class ForgotPasswordComponent {
  private fb = inject(UntypedFormBuilder);
  private accountService = inject(AccountService);

  form: UntypedFormGroup;

  inProgress?: boolean;

  isEmailSent = false;

  constructor() {
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
