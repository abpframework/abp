import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { AccountRoutingModule } from './account-routing.module';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { LoginComponent } from './components/login/login.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { PersonalSettingsComponent } from './components/personal-settings/personal-settings.component';
import { RegisterComponent } from './components/register/register.component';
import { AccountConfigOptions } from './models/config-options';
import { ACCOUNT_CONFIG_OPTIONS } from './tokens/config-options.token';
import { accountConfigOptionsFactory } from './utils/factory-utils';
import { AuthenticationFlowGuard } from './guards/authentication-flow.guard';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { UiExtensionsModule } from '@abp/ng.theme.shared/extensions';
import { ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS } from './tokens/extensions.token';
import { AccountExtensionsGuard } from './guards/extensions.guard';
import { PersonalSettingsHalfRowComponent } from './components/personal-settings/personal-settings-half-row.component';

const declarations = [
  LoginComponent,
  RegisterComponent,
  ChangePasswordComponent,
  ManageProfileComponent,
  PersonalSettingsComponent,
  ForgotPasswordComponent,
  ResetPasswordComponent,
  PersonalSettingsHalfRowComponent,
];

@NgModule({
  declarations: [...declarations],
  imports: [
    CoreModule,
    AccountRoutingModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    UiExtensionsModule,
  ],
  exports: [...declarations],
})
export class AccountModule {
  static forChild(options = {} as AccountConfigOptions): ModuleWithProviders<AccountModule> {
    return {
      ngModule: AccountModule,
      providers: [
        AuthenticationFlowGuard,
        { provide: ACCOUNT_CONFIG_OPTIONS, useValue: options },
        {
          provide: 'ACCOUNT_OPTIONS',
          useFactory: accountConfigOptionsFactory,
          deps: [ACCOUNT_CONFIG_OPTIONS],
        },
        {
          provide: ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
        AccountExtensionsGuard,
      ],
    };
  }

  static forLazy(options = {} as AccountConfigOptions): NgModuleFactory<AccountModule> {
    return new LazyModuleFactory(AccountModule.forChild(options));
  }
}
