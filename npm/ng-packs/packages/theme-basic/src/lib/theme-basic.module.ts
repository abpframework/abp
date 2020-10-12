import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { AccountLayoutComponent } from './components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from './components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from './components/empty-layout/empty-layout.component';
import { LogoComponent } from './components/logo/logo.component';
import { NavItemsComponent } from './components/nav-items/nav-items.component';
import { RoutesComponent } from './components/routes/routes.component';
import { ValidationErrorComponent } from './components/validation-error/validation-error.component';
import { CurrentUserComponent } from './components/nav-items/current-user.component';
import { LanguagesComponent } from './components/nav-items/languages.component';
import { BASIC_THEME_STYLES_PROVIDERS } from './providers/styles.provider';
import { BASIC_THEME_NAV_ITEM_PROVIDERS } from './providers/nav-item.provider';

export const LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];

@NgModule({
  declarations: [
    ...LAYOUTS,
    ValidationErrorComponent,
    LogoComponent,
    NavItemsComponent,
    RoutesComponent,
    CurrentUserComponent,
    LanguagesComponent,
  ],
  exports: [
    ...LAYOUTS,
    ValidationErrorComponent,
    LogoComponent,
    NavItemsComponent,
    RoutesComponent,
    CurrentUserComponent,
    LanguagesComponent,
  ],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbCollapseModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
  ],
  entryComponents: [...LAYOUTS, ValidationErrorComponent, CurrentUserComponent, LanguagesComponent],
})
export class ThemeBasicModule {
  static forRoot(): ModuleWithProviders<ThemeBasicModule> {
    return {
      ngModule: RootThemeBasicModule,
      providers: [BASIC_THEME_NAV_ITEM_PROVIDERS, BASIC_THEME_STYLES_PROVIDERS],
    };
  }
}

@NgModule({
  imports: [
    NgxValidateCoreModule.forRoot({
      targetSelector: '.form-group',
      blueprints: {
        creditCard: 'AbpValidation::ThisFieldIsNotAValidCreditCardNumber.',
        email: 'AbpValidation::ThisFieldIsNotAValidEmailAddress.',
        invalid: 'AbpValidation::ThisFieldIsNotValid.',
        max: 'AbpValidation::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
        maxlength:
          'AbpValidation::ThisFieldMustBeAStringOrArrayTypeWithAMaximumLengthOf{0}[{{ requiredLength }}]',
        min: 'AbpValidation::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
        minlength:
          'AbpValidation::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf{0}[{{ requiredLength }}]',
        ngbDate: 'AbpValidation::ThisFieldIsNotValid.',
        passwordMismatch: 'AbpIdentity::Volo.Abp.Identity:PasswordConfirmationFailed',
        range: 'AbpValidation::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
        required: 'AbpValidation::ThisFieldIsRequired.',
        url: 'AbpValidation::ThisFieldIsNotAValidFullyQualifiedHttpHttpsOrFtpUrl',
      },
      errorTemplate: ValidationErrorComponent,
    }),
  ],
})
export class RootThemeBasicModule {}
