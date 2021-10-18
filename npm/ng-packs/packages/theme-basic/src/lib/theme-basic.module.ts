import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import {
  NgxValidateCoreModule,
  VALIDATION_ERROR_TEMPLATE,
  VALIDATION_INVALID_CLASSES,
  VALIDATION_TARGET_SELECTOR,
} from '@ngx-validate/core';
import { AccountLayoutComponent } from './components/account-layout/account-layout.component';
import { AuthWrapperComponent } from './components/account-layout/auth-wrapper/auth-wrapper.component';
import { TenantBoxComponent } from './components/account-layout/tenant-box/tenant-box.component';
import { ApplicationLayoutComponent } from './components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from './components/empty-layout/empty-layout.component';
import { LogoComponent } from './components/logo/logo.component';
import { CurrentUserComponent } from './components/nav-items/current-user.component';
import { LanguagesComponent } from './components/nav-items/languages.component';
import { NavItemsComponent } from './components/nav-items/nav-items.component';
import { PageAlertContainerComponent } from './components/page-alert-container/page-alert-container.component';
import { RoutesComponent } from './components/routes/routes.component';
import { ValidationErrorComponent } from './components/validation-error/validation-error.component';
import { LazyStyleHandler } from './handlers/lazy-style.handler';
import { BASIC_THEME_NAV_ITEM_PROVIDERS } from './providers/nav-item.provider';
import { BASIC_THEME_STYLES_PROVIDERS } from './providers/styles.provider';

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
    PageAlertContainerComponent,
    TenantBoxComponent,
    AuthWrapperComponent,
  ],
  exports: [
    ...LAYOUTS,
    ValidationErrorComponent,
    LogoComponent,
    NavItemsComponent,
    RoutesComponent,
    CurrentUserComponent,
    LanguagesComponent,
    PageAlertContainerComponent,
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
export class BaseThemeBasicModule {}

@NgModule({
  exports: [BaseThemeBasicModule],
  imports: [BaseThemeBasicModule],
})
export class ThemeBasicModule {
  static forRoot(): ModuleWithProviders<ThemeBasicModule> {
    return {
      ngModule: ThemeBasicModule,
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
        {
          provide: VALIDATION_INVALID_CLASSES,
          useValue: 'is-invalid',
        },
        LazyStyleHandler,
      ],
    };
  }
}
