import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule, Provider } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { AccountRoutingModule } from './account-routing.module';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { LoginComponent } from './components/login/login.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { PersonalSettingsComponent } from './components/personal-settings/personal-settings.component';
import { RegisterComponent } from './components/register/register.component';
import { TenantBoxComponent } from './components/tenant-box/tenant-box.component';
import { Options } from './models/options';
import { ACCOUNT_OPTIONS, optionsFactory } from './tokens/options.token';
import { AuthWrapperComponent } from './components/auth-wrapper/auth-wrapper.component';

@NgModule({
  declarations: [
    AuthWrapperComponent,
    LoginComponent,
    RegisterComponent,
    TenantBoxComponent,
    ChangePasswordComponent,
    ManageProfileComponent,
    PersonalSettingsComponent,
  ],
  imports: [
    CoreModule,
    AccountRoutingModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
  ],
  exports: [],
})
export class AccountModule {}
