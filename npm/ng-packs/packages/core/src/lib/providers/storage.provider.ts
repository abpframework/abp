import { AbpStorageService, AbpCookieStorageService, AbpLocalStorageService } from '../services/';
import { Provider } from '@angular/core';

export const cookieBasedStorageProvider: Provider = {
  provide: AbpStorageService,
  useClass: AbpCookieStorageService,
};

export const localStorageBasedStorageProvider: Provider = {
  provide: AbpStorageService,
  useClass: AbpLocalStorageService,
};
