import {
  AuthService,
  ConfigStateService,
  CurrentUserDto,
  LocalizationPipe,
  NAVIGATE_TO_MANAGE_PROFILE,
  PermissionDirective,
  SessionStateService,
  ToInjectorPipe,
} from '@abp/ng.core';
import { AbpVisibleDirective, UserMenu, UserMenuService } from '@abp/ng.theme.shared';
import { Component, Inject, TrackByFunction } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'abp-current-user',
  templateUrl: './current-user.component.html',
  imports: [
    CommonModule,
    NgbDropdownModule,
    AbpVisibleDirective,
    PermissionDirective,
    ToInjectorPipe,
    LocalizationPipe,
  ],
})
export class CurrentUserComponent {
  currentUser$: Observable<CurrentUserDto> = this.configState.getOne$('currentUser');
  selectedTenant$ = this.sessionState.getTenant$();

  trackByFn: TrackByFunction<UserMenu> = (_, element) => element.id;

  get smallScreen(): boolean {
    return window.innerWidth < 992;
  }

  constructor(
    @Inject(NAVIGATE_TO_MANAGE_PROFILE) public readonly navigateToManageProfile: () => void,
    public readonly userMenu: UserMenuService,
    private authService: AuthService,
    private configState: ConfigStateService,
    private sessionState: SessionStateService,
  ) {}

  navigateToLogin() {
    this.authService.navigateToLogin();
  }

  logout() {
    this.authService.logout().subscribe();
  }
}
