import { ApplicationConfiguration, ConfigState, AuthService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';

@Component({
  selector: 'abp-current-user',
  // tslint:disable-next-line: component-max-inline-declarations
  template: `
    <ng-template #loginBtn>
      <a role="button" class="nav-link" routerLink="/account/login">{{
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
        <a class="dropdown-item" routerLink="/account/manage-profile"
          ><i class="fa fa-cog mr-1"></i>{{ 'AbpAccount::ManageYourProfile' | abpLocalization }}</a
        >
        <a class="dropdown-item" href="javascript:void(0)" (click)="logout()"
          ><i class="fa fa-power-off mr-1"></i>{{ 'AbpUi::Logout' | abpLocalization }}</a
        >
      </div>
    </div>
  `,
})
export class CurrentUserComponent implements OnInit {
  @Select(ConfigState.getOne('currentUser'))
  currentUser$: Observable<ApplicationConfiguration.CurrentUser>;

  get smallScreen(): boolean {
    return window.innerWidth < 992;
  }

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {}

  logout() {
    this.authService.logout().subscribe(() => {
      this.router.navigate(['/'], { state: { redirectUrl: this.router.url } });
    });
  }
}
