import { ProfileDto, ProfileService } from '@abp/ng.account.core/proxy';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { Component, inject, Injector, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { finalize, filter } from 'rxjs/operators';
import { Account } from '../../models/account';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';
import { AuthService, ConfigStateService } from '@abp/ng.core';
import { RE_LOGIN_CONFIRMATION_TOKEN } from '../../tokens';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { eAccountComponents } from '../../enums';

@Component({
  selector: 'abp-personal-settings-form',
  templateUrl: './personal-settings.component.html',
  exportAs: 'abpPersonalSettingsForm',
  providers: [
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eAccountComponents.PersonalSettings,
    },
  ],
})
export class PersonalSettingsComponent
  implements
    OnInit,
    Account.PersonalSettingsComponentInputs,
    Account.PersonalSettingsComponentOutputs
{
  private readonly fb = inject(UntypedFormBuilder);
  protected readonly toasterService = inject(ToasterService);
  protected readonly profileService = inject(ProfileService);
  protected readonly manageProfileState = inject(ManageProfileStateService);
  protected readonly authService = inject(AuthService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly isPersonalSettingsChangedConfirmationActive = inject(
    RE_LOGIN_CONFIRMATION_TOKEN,
  );
  private readonly injector = inject(Injector);

  selected?: ProfileDto;

  form!: UntypedFormGroup;

  inProgress?: boolean;

  buildForm() {
    this.selected = this.manageProfileState.getProfile();
    if (!this.selected) {
      return;
    }
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);
  }

  ngOnInit(): void {
    this.buildForm();
  }

  submit() {
    if (this.form.invalid) return;
    const isLogOutConfirmMessageVisible = this.isLogoutConfirmMessageActive();
    const isRefreshTokenExists = this.authService.getRefreshToken();
    this.inProgress = true;
    this.profileService
      .update(this.form.value)
      .pipe(finalize(() => (this.inProgress = false)))
      .subscribe(profile => {
        this.manageProfileState.setProfile(profile);
        this.configState.refreshAppState();
        this.toasterService.success('AbpAccount::PersonalSettingsSaved', 'Success', { life: 5000 });

        if (isRefreshTokenExists) {
          return this.authService.refreshToken();
        }

        if (isLogOutConfirmMessageVisible) {
          this.showLogoutConfirmMessage();
        }
      });
  }

  logoutConfirmation = () => {
    this.authService.logout().subscribe();
  };

  private isLogoutConfirmMessageActive() {
    return this.isPersonalSettingsChangedConfirmationActive;
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
