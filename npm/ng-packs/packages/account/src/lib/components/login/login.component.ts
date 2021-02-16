import { ConfigStateService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import snq from 'snq';
import { eAccountComponents } from '../../enums/components';
import { AuthenticationService } from '../../services/authentication.service';

const { maxLength, minLength, required } = Validators;

@Component({
  selector: 'abp-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  form: FormGroup;

  inProgress: boolean;

  isSelfRegistrationEnabled = true;

  authWrapperKey = eAccountComponents.AuthWrapper;

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private toasterService: ToasterService,
    private authenticationService: AuthenticationService,
    private configState: ConfigStateService,
  ) {}

  ngOnInit() {
    this.init();
    this.buildForm();
  }

  protected init() {
    this.isSelfRegistrationEnabled =
      (
        (this.configState.getSetting('Abp.Account.IsSelfRegistrationEnabled') as string) || ''
      ).toLowerCase() !== 'false';
  }

  protected buildForm() {
    this.form = this.fb.group({
      username: ['', [required, maxLength(255)]],
      password: ['', [required, maxLength(128)]],
      remember: [false],
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.inProgress = true;

    const { username, password, remember } = this.form.value;

    this.authenticationService
      .login(username, password, remember)
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
        // TODO: this.store.dispatch(new SetRemember(this.form.get('remember').value));
      });
  }
}
