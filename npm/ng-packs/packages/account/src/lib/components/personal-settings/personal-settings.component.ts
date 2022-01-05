import { ProfileService } from '@abp/ng.account.core/proxy';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { Account } from '../../models/account';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';

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
    Account.PersonalSettingsComponentOutputs
{
  form: FormGroup;

  inProgress: boolean;

  constructor(
    private fb: FormBuilder,
    private toasterService: ToasterService,
    private profileService: ProfileService,
    private manageProfileState: ManageProfileStateService,
  ) {}

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    const profile = this.manageProfileState.getProfile();
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
    this.profileService
      .update(this.form.value)
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(profile => {
        this.manageProfileState.setProfile(profile);
        this.toasterService.success('AbpAccount::PersonalSettingsSaved', 'Success', { life: 5000 });
      });
  }
}
