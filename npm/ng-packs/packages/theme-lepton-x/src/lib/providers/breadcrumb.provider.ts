import { APP_INITIALIZER, Injector } from '@angular/core';
import { AbpBreadcrumbService } from '../services/breadcrumb.service';

export const LPX_BREADCRUMB_PROVIDER = {
  provide: APP_INITIALIZER,
  multi: true,
  deps: [Injector],
  useFactory: initBreadcrumb,
};

function initBreadcrumb(injector: Injector) {
  const userProfile = injector.get(AbpBreadcrumbService);
  return () => {
    userProfile.listenRouter();
  };
}
