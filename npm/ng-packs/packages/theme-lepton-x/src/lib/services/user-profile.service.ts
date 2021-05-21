import { Inject, Injectable } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import {
  AuthService,
  ConfigStateService,
  CurrentUserDto,
  LocalizationService,
  NAVIGATE_TO_MANAGE_PROFILE,
} from '@abp/ng.core';
import { UserProfileService } from '@lepton-x/common';
import { filter } from 'rxjs/operators';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({
  providedIn: 'root',
})
export class AbpUserProfileService {
  currentUser$: Observable<CurrentUserDto> = this.configState.getOne$('currentUser');

  constructor(
    private configState: ConfigStateService,
    private userProfileService: UserProfileService,
    private localizationService: LocalizationService,
    private authService: AuthService,
    @Inject(NAVIGATE_TO_MANAGE_PROFILE) public navigateToManageProfile,
    private oAuthService: OAuthService,
  ) {}

  subscribeUser() {
    combineLatest(
      this.currentUser$.pipe(filter<CurrentUserDto>(Boolean)),
      this.localizationService.get('AbpAccount::ManageYourProfile'),
      this.localizationService.get('AbpUi::Logout'),
      this.localizationService.get('AbpUi::Login'),
    ).subscribe(([user, manageText, logoutText, loginText]) => {
      this.userProfileService.setUser({
        fullName: user.name || user.userName,
        email: user.email,
        userName: user.userName,
        avatar: {
          type: 'icon',
          source: 'bi bi-person-circle',
        },
        userActionGroups: [
          [
            ...(this.oAuthService.hasValidAccessToken()
              ? [
                  {
                    text: manageText,
                    action: () => this.navigateToManageProfile(),
                  },
                  {
                    text: logoutText,
                    action: () => this.authService.logout().subscribe(),
                  },
                ]
              : [
                  {
                    text: loginText,
                    action: () => this.authService.navigateToLogin(),
                  },
                ]),
          ],
        ],
      });
    });
  }
}
