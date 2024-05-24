import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
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
import { provideThemeBasicConfig } from './providers';

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
