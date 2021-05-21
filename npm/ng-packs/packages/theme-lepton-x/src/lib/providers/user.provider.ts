import { APP_INITIALIZER, Injector } from '@angular/core';
import { AbpUserProfileService } from '../services';

export const LPX_USER_PROVIDER = {
  provide: APP_INITIALIZER,
  multi: true,
  deps: [Injector],
  useFactory: initUser,
};

function initUser(injector: Injector) {
  const userProfile = injector.get(AbpUserProfileService);
  return () => {
    userProfile.subscribeUser();
  };
}
