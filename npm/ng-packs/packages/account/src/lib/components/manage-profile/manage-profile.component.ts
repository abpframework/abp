import { ProfileService } from '@abp/ng.account.core/proxy';
import { LoadingDirective } from '@abp/ng.theme.shared';
import { Component, inject, OnInit } from '@angular/core';
import { eAccountComponents } from '../../enums/components';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';
import { AsyncPipe } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { LocalizationPipe, ReplaceableTemplateDirective } from '@abp/ng.core';
import { PersonalSettingsComponent } from '../personal-settings/personal-settings.component';
import { ChangePasswordComponent } from '../change-password/change-password.component';

@Component({
  selector: 'abp-manage-profile',
  templateUrl: './manage-profile.component.html',
  styles: [
    `
      .min-h-400 {
        min-height: 400px;
      }

      .fade-in {
        animation: fadeIn 350ms ease both;
      }

      @keyframes fadeIn {
        from {
          opacity: 0;
        }
        to {
          opacity: 1;
        }
      }
    `,
  ],
  imports: [
    AsyncPipe,
    ReactiveFormsModule,
    PersonalSettingsComponent,
    ChangePasswordComponent,
    LocalizationPipe,
    ReplaceableTemplateDirective,
    LoadingDirective
  ],
})
export class ManageProfileComponent implements OnInit {
  protected profileService = inject(ProfileService);
  protected manageProfileState = inject(ManageProfileStateService);

  selectedTab = 0;

  changePasswordKey = eAccountComponents.ChangePassword;

  personalSettingsKey = eAccountComponents.PersonalSettings;

  profile$ = this.manageProfileState.getProfile$();

  hideChangePasswordTab?: boolean;

  ngOnInit() {
    this.profileService.get().subscribe(profile => {
      this.manageProfileState.setProfile(profile);
      if (profile.isExternal) {
        this.hideChangePasswordTab = true;
        this.selectedTab = 1;
      }
    });
  }
}
