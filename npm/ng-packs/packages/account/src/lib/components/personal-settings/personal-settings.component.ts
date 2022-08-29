import { ProfileDto, ProfileService } from '@abp/ng.account.core/proxy';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { Account } from '../../models/account';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.theme.shared/extensions';
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
  selected: ProfileDto;

  form: FormGroup;

  inProgress: boolean;

  constructor(
    private fb: FormBuilder,
    private toasterService: ToasterService,
    private profileService: ProfileService,
    private manageProfileState: ManageProfileStateService,
    protected injector: Injector,
  ) {}

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
