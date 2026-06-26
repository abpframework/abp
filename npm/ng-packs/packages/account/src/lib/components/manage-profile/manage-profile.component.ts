import { ProfileService } from '@abp/ng.account.core/proxy';
import { LoadingDirective } from '@abp/ng.theme.shared';
import { Component, inject, OnInit, signal, ChangeDetectionStrategy } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { eAccountComponents } from '../../enums/components';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';
import { ReactiveFormsModule } from '@angular/forms';
import { LocalizationPipe, ReplaceableTemplateDirective } from '@abp/ng.core';
import { PersonalSettingsComponent } from '../personal-settings/personal-settings.component';
import { ChangePasswordComponent } from '../change-password/change-password.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
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
    LoadingDirective,
  ],
})
export class ManageProfileComponent implements OnInit {
  protected profileService = inject(ProfileService);
  protected manageProfileState = inject(ManageProfileStateService);

  readonly selectedTab = signal(0);
  readonly hideChangePasswordTab = signal(false);

  readonly profile = toSignal(this.manageProfileState.getProfile$());

  changePasswordKey = eAccountComponents.ChangePassword;

  personalSettingsKey = eAccountComponents.PersonalSettings;

  ngOnInit() {
    this.profileService.get().subscribe(profile => {
      this.manageProfileState.setProfile(profile);
      if (profile.isExternal) {
        this.hideChangePasswordTab.set(true);
        this.selectedTab.set(1);
      }
    });
  }
}
