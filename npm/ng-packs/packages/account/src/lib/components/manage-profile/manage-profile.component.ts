import { ProfileService } from '@abp/ng.account.core/proxy';
import { LoadingDirective } from '@abp/ng.theme.shared';
import { Component, inject, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs/operators';
import { eAccountComponents } from '../../enums/components';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';
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

  profile = toSignal(this.manageProfileState.getProfile$());

  profileLoading = signal(false);

  hideChangePasswordTab?: boolean;

  ngOnInit() {
    this.profileLoading.set(true);
    this.profileService
      .get()
      .pipe(finalize(() => this.profileLoading.set(false)))
      .subscribe(profile => {
        this.manageProfileState.setProfile(profile);
        if (profile.isExternal) {
          this.hideChangePasswordTab = true;
          this.selectedTab = 1;
        }
      });
  }
}
