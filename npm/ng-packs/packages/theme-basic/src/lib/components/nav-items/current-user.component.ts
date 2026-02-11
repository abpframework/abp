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
import { Component, TrackByFunction, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { NgComponentOutlet, AsyncPipe, DOCUMENT } from '@angular/common';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'abp-current-user',
  templateUrl: './current-user.component.html',
  imports: [
    NgComponentOutlet,
    AsyncPipe,
    NgbDropdownModule,
    AbpVisibleDirective,
    PermissionDirective,
    ToInjectorPipe,
    LocalizationPipe,
  ],
})
export class CurrentUserComponent {
  readonly navigateToManageProfile = inject(NAVIGATE_TO_MANAGE_PROFILE);
  readonly userMenu = inject(UserMenuService);
  private authService = inject(AuthService);
  private configState = inject(ConfigStateService);
  private sessionState = inject(SessionStateService);
  private document = inject(DOCUMENT);

  currentUser$: Observable<CurrentUserDto> = this.configState.getOne$('currentUser');
  selectedTenant$ = this.sessionState.getTenant$();
  trackByFn: TrackByFunction<UserMenu> = (_, element) => element.id;

  get smallScreen(): boolean {
    return this.document.defaultView?.innerWidth < 992;
  }

  navigateToLogin() {
    this.authService.navigateToLogin();
  }

  logout() {
    this.authService.logout().subscribe();
  }
}
