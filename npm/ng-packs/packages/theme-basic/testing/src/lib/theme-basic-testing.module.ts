import {
  BaseThemeBasicModule,
  BASIC_THEME_NAV_ITEM_PROVIDERS,
  BASIC_THEME_STYLES_PROVIDERS,
  ValidationErrorComponent,
} from '@abp/ng.theme.basic';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { VALIDATION_ERROR_TEMPLATE, VALIDATION_TARGET_SELECTOR } from '@ngx-validate/core';

@NgModule({
  exports: [BaseThemeBasicModule],
  imports: [BaseThemeBasicModule],
})
export class ThemeBasicTestingModule {
  static forRoot(): ModuleWithProviders<ThemeBasicTestingModule> {
    return {
      ngModule: ThemeBasicTestingModule,
      providers: [
        BASIC_THEME_NAV_ITEM_PROVIDERS,
        BASIC_THEME_STYLES_PROVIDERS,
        {
          provide: VALIDATION_ERROR_TEMPLATE,
          useValue: ValidationErrorComponent,
        },
        {
          provide: VALIDATION_TARGET_SELECTOR,
          useValue: '.form-group',
        },
      ],
    };
  }
}
