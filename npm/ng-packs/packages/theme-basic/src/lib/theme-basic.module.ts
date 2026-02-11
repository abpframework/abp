import { ModuleWithProviders, NgModule } from '@angular/core';
import {
  AccountLayoutComponent,
  AuthWrapperComponent,
  TenantBoxComponent,
  ApplicationLayoutComponent,
  EmptyLayoutComponent,
  LogoComponent,
  CurrentUserComponent,
  LanguagesComponent,
  NavItemsComponent,
  PageAlertContainerComponent,
  RoutesComponent,
  ValidationErrorComponent,
} from './components';
import { provideThemeBasicConfig } from './providers';

export const LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];

export const THEME_BASIC_COMPONENTS = [
  ...LAYOUTS,
  ValidationErrorComponent,
  LogoComponent,
  NavItemsComponent,
  RoutesComponent,
  CurrentUserComponent,
  LanguagesComponent,
  PageAlertContainerComponent,
  AuthWrapperComponent,
  TenantBoxComponent,
];

@NgModule({
  declarations: [],
  exports: [...THEME_BASIC_COMPONENTS],
  imports: [...THEME_BASIC_COMPONENTS],
})
export class BaseThemeBasicModule {}

@NgModule({
  exports: [BaseThemeBasicModule],
  imports: [BaseThemeBasicModule],
})
export class ThemeBasicModule {
  /**
   * @deprecated forRoot method is deprecated, use `provideThemeBasicConfig` *function* for config settings.
   */
  static forRoot(): ModuleWithProviders<ThemeBasicModule> {
    return {
      ngModule: ThemeBasicModule,
      providers: [provideThemeBasicConfig()],
    };
  }
}
