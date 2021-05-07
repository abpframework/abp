import { APP_INITIALIZER, Injector, Provider } from '@angular/core';
import { NavbarService } from '@abp/ng.theme-lepton-x';

export const LPX_NAVBAR_ITEMS_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  multi: true,
  useFactory: initNavbarItems,
  deps: [Injector],
};

function initNavbarItems(injector: Injector) {
  const navbarService = injector.get(NavbarService);
  return () => {
    navbarService.initRoutes();
  };
}
