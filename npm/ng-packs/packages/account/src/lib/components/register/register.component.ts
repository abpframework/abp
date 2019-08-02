import { ToasterService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { validatePassword } from '@ngx-validate/core';
import { throwError } from 'rxjs';
import { catchError, finalize, take } from 'rxjs/operators';
import snq from 'snq';
import { RegisterRequest } from '../../models';
import { AccountService } from '../../services/account.service';

const { maxLength, minLength, required, email } = Validators;

@Component({
  selector: 'abp-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  form: FormGroup;

  inProgress: boolean;

  constructor(private fb: FormBuilder, private accountService: AccountService, private toasterService: ToasterService) {
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

    this.inProgress = true;

    const newUser = {
      userName: this.form.get('username').value,
      password: this.form.get('password').value,
      emailAddress: this.form.get('email').value,
      appName: 'angular',
    } as RegisterRequest;

    this.accountService
      .register(newUser)
      .pipe(
        take(1),
        catchError(err => {
          this.toasterService.error(snq(() => err.error.error_description, 'An error occured.'), 'Error');
          return throwError(err);
        }),
        finalize(() => (this.inProgress = false)),
      )
      .subscribe();
  }
}
