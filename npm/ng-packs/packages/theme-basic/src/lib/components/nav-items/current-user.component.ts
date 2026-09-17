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
import { ChangeDetectionStrategy, Component, inject, TrackByFunction } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { DOCUMENT, NgComponentOutlet } from '@angular/common';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-current-user',
  templateUrl: './current-user.component.html',
  imports: [
    NgComponentOutlet,
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

  readonly currentUser = toSignal(this.configState.getOne$('currentUser'), {
    initialValue: {} as CurrentUserDto,
  });
  readonly selectedTenant = toSignal(this.sessionState.getTenant$());
  readonly userMenuItems = toSignal(this.userMenu.items$, { initialValue: [] as UserMenu[] });

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
