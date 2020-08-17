import { ProfileState, UpdateProfile } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { finalize } from 'rxjs/operators';
import { Account } from '../../models/account';

const { maxLength, required, email } = Validators;

@Component({
  selector: 'abp-personal-settings-form',
  templateUrl: './personal-settings.component.html',
  exportAs: 'abpPersonalSettingsForm',
})
export class PersonalSettingsComponent
  implements
    OnInit,
    Account.PersonalSettingsComponentInputs,
    Account.PersonalSettingsComponentOutputs {
  form: FormGroup;

  inProgress: boolean;

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private toasterService: ToasterService,
  ) {}

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    const profile = this.store.selectSnapshot(ProfileState.getProfile);

    this.form = this.fb.group({
      userName: [profile.userName, [required, maxLength(256)]],
      email: [profile.email, [required, email, maxLength(256)]],
      name: [profile.name || '', [maxLength(64)]],
      surname: [profile.surname || '', [maxLength(64)]],
      phoneNumber: [profile.phoneNumber || '', [maxLength(16)]],
    });
  }

  submit() {
    if (this.form.invalid) return;
    this.inProgress = true;
    this.store
      .dispatch(new UpdateProfile(this.form.value))
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(() => {
        this.toasterService.success('AbpAccount::PersonalSettingsSaved', 'Success', { life: 5000 });
      });
  }
}
