import {
  BaseThemeSharedModule,
  DateParserFormatter,
  THEME_SHARED_ROUTE_PROVIDERS,
} from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';

/**
 * ThemeSharedTestingModule is the module that will be used in tests
 */
@NgModule({
  exports: [RouterTestingModule, BaseThemeSharedModule],
  imports: [RouterTestingModule, BaseThemeSharedModule],
})
export class ThemeSharedTestingModule {
  static withConfig(): ModuleWithProviders<ThemeSharedTestingModule> {
    return {
      ngModule: ThemeSharedTestingModule,
      providers: [
        THEME_SHARED_ROUTE_PROVIDERS,
        { provide: NgbDateParserFormatter, useClass: DateParserFormatter },
      ],
    };
  }
}
