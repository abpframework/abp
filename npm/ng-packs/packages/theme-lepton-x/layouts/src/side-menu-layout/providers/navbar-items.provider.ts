import { APP_INITIALIZER, Injector, Provider } from '@angular/core';
import { AbpNavbarService } from '@abp/ng.theme.lepton-x';

export const LPX_NAVBAR_ITEMS_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  multi: true,
  useFactory: initNavbarItems,
  deps: [Injector],
};

function initNavbarItems(injector: Injector) {
  const navbarService = injector.get(AbpNavbarService);
  return () => {
    navbarService.initRoutes();
  };
}
