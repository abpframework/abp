import { APP_INITIALIZER, Injector, Provider } from '@angular/core';
import { NavItemsService } from '@abp/ng.theme.shared';
import { eThemeLeptonXComponents } from '@abp/ng.theme.lepton-x';
import { LanguageSelectionComponent } from '../components/language-selection/language-selection.component';
import { UserProfileComponent } from '../components/user-profile/user-profile.component';

export const NAV_ITEM_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  multi: true,
  useFactory: addNavItems,
  deps: [Injector],
};

export function addNavItems(injector: Injector) {
  return () => {
    const navItems = injector.get(NavItemsService);
    navItems.addItems([
      {
        id: eThemeLeptonXComponents.Languages,
        order: 100,
        component: LanguageSelectionComponent,
      },
      {
        id: eThemeLeptonXComponents.CurrentUser,
        order: 100,
        component: UserProfileComponent,
      },
    ]);
  };
}
