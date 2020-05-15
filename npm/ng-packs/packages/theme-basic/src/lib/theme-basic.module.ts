import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { AccountLayoutComponent } from './components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from './components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from './components/empty-layout/empty-layout.component';
import { LayoutState } from './states/layout.state';
import { ValidationErrorComponent } from './components/validation-error/validation-error.component';
import { InitialService } from './services/initial.service';
import { LogoComponent } from './components/logo/logo.component';
import { RoutesComponent } from './components/routes/routes.component';
import { NavItemsComponent } from './components/nav-items/nav-items.component';

export const LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];

@NgModule({
  declarations: [
    ...LAYOUTS,
    ValidationErrorComponent,
    LogoComponent,
    NavItemsComponent,
    RoutesComponent,
  ],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbCollapseModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    NgxsModule.forFeature([LayoutState]),
    NgxValidateCoreModule.forRoot({
      targetSelector: '.form-group',
      blueprints: {
        email: 'AbpAccount::ThisFieldIsNotAValidEmailAddress.',
        max: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
        maxlength:
          'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMaximumLengthOf{0}[{{ requiredLength }}]',
        min: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
        minlength:
          'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf{0}[{{ requiredLength }}]',
        required: 'AbpAccount::ThisFieldIsRequired.',
        passwordMismatch: 'AbpIdentity::Identity.PasswordConfirmationFailed',
      },
      errorTemplate: ValidationErrorComponent,
    }),
  ],
  exports: [...LAYOUTS, LogoComponent, ValidationErrorComponent],
  entryComponents: [...LAYOUTS, ValidationErrorComponent],
})
export class ThemeBasicModule {
  constructor(private initialService: InitialService) {}
}
