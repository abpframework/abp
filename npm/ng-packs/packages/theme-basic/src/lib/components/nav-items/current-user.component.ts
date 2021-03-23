import {
  AuthService,
  ConfigStateService,
  CurrentUserDto,
  NAVIGATE_TO_MANAGE_PROFILE,
} from '@abp/ng.core';
import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'abp-current-user',
  // tslint:disable-next-line: component-max-inline-declarations
  template: `
    <ng-template #loginBtn>
      <a role="button" class="nav-link pointer" (click)="navigateToLogin()">{{
        'AbpAccount::Login' | abpLocalization
      }}</a>
    </ng-template>
    <div
      *ngIf="(currentUser$ | async)?.isAuthenticated; else loginBtn"
      ngbDropdown
      class="dropdown"
      #currentUserDropdown="ngbDropdown"
      display="static"
    >
      <a
        ngbDropdownToggle
        class="nav-link"
        href="javascript:void(0)"
        role="button"
        id="dropdownMenuLink"
        data-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded="false"
      >
        {{ (currentUser$ | async)?.userName }}
      </a>
      <div
        class="dropdown-menu dropdown-menu-right border-0 shadow-sm"
        aria-labelledby="dropdownMenuLink"
        [class.d-block]="smallScreen && currentUserDropdown.isOpen()"
      >
        <a class="dropdown-item pointer" (click)="navigateToManageProfile()"
          ><i class="fa fa-cog mr-1"></i>{{ 'AbpAccount::ManageYourProfile' | abpLocalization }}</a
        >
        <a class="dropdown-item" href="javascript:void(0)" (click)="logout()"
          ><i class="fa fa-power-off mr-1"></i>{{ 'AbpUi::Logout' | abpLocalization }}</a
        >
      </div>
    </div>
  `,
})
export class CurrentUserComponent {
  currentUser$: Observable<CurrentUserDto> = this.configState.getOne$('currentUser');

  get smallScreen(): boolean {
    return window.innerWidth < 992;
  }

  constructor(
    @Inject(NAVIGATE_TO_MANAGE_PROFILE) public navigateToManageProfile,
    private authService: AuthService,
    private configState: ConfigStateService,
  ) {}

  navigateToLogin() {
    this.authService.navigateToLogin();
  }

  logout() {
    this.authService.logout().subscribe();
  }
}
