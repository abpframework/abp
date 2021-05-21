import { Injectable } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { ConfigStateService, CurrentUserDto, LocalizationService } from '@abp/ng.core';
import { UserProfileService } from '@lepton-x/common';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AbpUserProfileService {
  currentUser$: Observable<CurrentUserDto> = this.configState.getOne$('currentUser');

  constructor(
    private configState: ConfigStateService,
    private userProfileService: UserProfileService,
    private localizationService: LocalizationService,
  ) {}

  subscribeUser() {
    combineLatest(
      this.currentUser$.pipe(filter<CurrentUserDto>(Boolean)),
      this.localizationService.get('AbpAccount::ManageYourProfile'),
      this.localizationService.get('AbpUi::Logout'),
    ).subscribe(([user, manageText, logoutText]) => {
      this.userProfileService.setUser({
        fullName: user.name || user.userName,
        email: user.email,
        userName: user.userName,
        avatar: {
          type: 'icon',
          source: 'bi bi-person-circle',
        },
        // TODO implement manage profile links
        userActionGroups: [[{ text: manageText }, { text: logoutText }]],
      });
    });
  }
}
