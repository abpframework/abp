import { Profile, ProfileService } from '@abp/ng.core';
import { fadeIn } from '@abp/ng.theme.shared';
import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { eAccountComponents } from '../../enums/components';
import { ManageProfileStateService } from '../../services/manage-profile.state.service';

@Component({
  selector: 'abp-manage-profile',
  templateUrl: './manage-profile.component.html',
  animations: [trigger('fadeIn', [transition(':enter', useAnimation(fadeIn))])],
  styles: [
    `
      .min-h-400 {
        min-height: 400px;
      }
    `,
  ],
})
export class ManageProfileComponent implements OnInit {
  selectedTab = 0;

  changePasswordKey = eAccountComponents.ChangePassword;

  personalSettingsKey = eAccountComponents.PersonalSettings;

  profile$ = this.manageProfileState.getProfile$();

  hideChangePasswordTab: boolean;

  constructor(
    protected profileService: ProfileService,
    protected manageProfileState: ManageProfileStateService,
  ) {}

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
