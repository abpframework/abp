import {
  AuthService,
  ConfigStateService,
  LocalizationService,
  NAVIGATE_TO_MANAGE_PROFILE,
} from '@abp/ng.core';
import { UserMenuService } from '@abp/ng.theme.shared';
import { APP_INITIALIZER, Injector } from '@angular/core';
import { filter } from 'rxjs/operators';
import { eUserMenuItems } from '../enums/user-menu-items';

export const BASIC_THEME_USER_MENU_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureUserMenu,
    deps: [Injector],
    multi: true,
  },
];

export function configureUserMenu(injector: Injector) {
  const configState = injector.get(ConfigStateService);
  const userMenu = injector.get(UserMenuService);
  const localization = injector.get(LocalizationService);
  const authService = injector.get(AuthService);
  const navigateToManageProfile = injector.get(NAVIGATE_TO_MANAGE_PROFILE);

  return () => {
    configState
      .getAll$()
      .pipe(filter(res => !!res?.localization))
      .subscribe(() => {
        userMenu.addItems([
          {
            id: eUserMenuItems.MyAccount,
            order: 100,
            html: `
          <a class="dropdown-item pointer"
          ><i class="fa fa-cog me-1"></i>${localization.instant('AbpAccount::MyAccount')}</a
        >
          `,
            action: () => navigateToManageProfile(),
          },
          {
            id: eUserMenuItems.Logout,
            order: 101,
            html: `
          <a class="dropdown-item" href="javascript:void(0)"
          ><i class="fa fa-power-off me-1"></i>${localization.instant('AbpUi::Logout')}</a
        >
          `,
            action: () => {
              authService.logout().subscribe();
            },
          },
        ]);
      });
  };
}
