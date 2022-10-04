import { ProfileDto, ProfileService } from '@abp/ng.account.core/proxy';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize, filter } from 'rxjs/operators';
import { Account } from '../../models/account';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';
import { AuthService } from '@abp/ng.core';
import { RE_LOGIN_CONFIRMATION_TOKEN } from '../../tokens';

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
  private profile: ProfileDto;

  constructor(
    private fb: FormBuilder,
    private toasterService: ToasterService,
    private profileService: ProfileService,
    private manageProfileState: ManageProfileStateService,
    private readonly authService: AuthService,
    private confirmationService: ConfirmationService,
    @Inject(RE_LOGIN_CONFIRMATION_TOKEN)
    private isPersonalSettingsChangedConfirmationActive: boolean,
  ) {}

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    this.profile = this.manageProfileState.getProfile();
    this.form = this.fb.group({
      userName: [this.profile.userName, [required, maxLength(256)]],
      email: [this.profile.email, [required, email, maxLength(256)]],
      name: [this.profile.name || '', [maxLength(64)]],
      surname: [this.profile.surname || '', [maxLength(64)]],
      phoneNumber: [this.profile.phoneNumber || '', [maxLength(16)]],
    });
  }

  submit() {
    if (this.form.invalid) return;
    const isLogOutConfirmMessageVisible = this.isLogoutConfirmMessageActive();
    this.inProgress = true;
    this.profileService
      .update(this.form.value)
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(profile => {
        this.manageProfileState.setProfile(profile);
        this.toasterService.success('AbpAccount::PersonalSettingsSaved', 'Success', { life: 5000 });
        if (isLogOutConfirmMessageVisible) {
          this.showLogoutConfirmMessage();
        }
      });
  }

  isDataSame(oldValue, newValue) {
    return Object.entries(oldValue).some(([key, value]) => {
      if (key in newValue) {
        return value !== newValue[key];
      }
      return false;
    });
  }

  logoutConfirmation = () => {
    this.authService.logout().subscribe();
  };

  private isLogoutConfirmMessageActive() {
    if (!this.isPersonalSettingsChangedConfirmationActive) {
      return false;
    }
    return this.isDataSame(this.profile, this.form.value);
  }

  private showLogoutConfirmMessage() {
    this.confirmationService
      .info(
        'AbpAccount::PersonalSettingsChangedConfirmationModalDescription',
        'AbpAccount::PersonalSettingsChangedConfirmationModalTitle',
      )
      .pipe(filter(status => status === Confirmation.Status.confirm))
      .subscribe(this.logoutConfirmation);
  }
}
