import { makeEnvironmentProviders, inject, provideAppInitializer } from '@angular/core';

import {
  VALIDATION_ERROR_TEMPLATE,
  VALIDATION_TARGET_SELECTOR,
  VALIDATION_INVALID_CLASSES,
} from '@ngx-validate/core';
import { LazyStyleHandler } from '../handlers';
import { BASIC_THEME_NAV_ITEM_PROVIDERS } from './nav-item.provider';
import { BASIC_THEME_STYLES_PROVIDERS } from './styles.provider';
import { BASIC_THEME_USER_MENU_PROVIDERS } from './user-menu.provider';
import { ValidationErrorComponent } from '../components';

export function provideThemeBasicConfig() {
  return makeEnvironmentProviders([
    BASIC_THEME_NAV_ITEM_PROVIDERS,
    BASIC_THEME_USER_MENU_PROVIDERS,
    BASIC_THEME_STYLES_PROVIDERS,
    {
      provide: VALIDATION_ERROR_TEMPLATE,
      useValue: ValidationErrorComponent,
    },
    {
      provide: VALIDATION_TARGET_SELECTOR,
      useValue: '.form-group',
    },
    {
      provide: VALIDATION_INVALID_CLASSES,
      useValue: 'is-invalid',
    },
    LazyStyleHandler,
    provideAppInitializer(() => {
      inject(LazyStyleHandler);
    }),
  ]);
}
