import { fadeIn } from '@abp/ng.theme.shared';
import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { eAccountComponents } from '../../enums/components';
import { Store } from '@ngxs/store';
import { GetProfile, ProfileState } from '@abp/ng.core';

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

  isProfileLoaded: boolean;

  hideChangePasswordTab: boolean;

  constructor(private store: Store) {}

  ngOnInit() {
    this.store.dispatch(new GetProfile()).subscribe(() => {
      this.isProfileLoaded = true;
      if (this.store.selectSnapshot(ProfileState.getProfile).isExternal) {
        this.hideChangePasswordTab = true;
        this.selectedTab = 1;
      }
    });
  }
}
