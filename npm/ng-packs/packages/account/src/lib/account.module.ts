import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule, Provider } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { TableModule } from 'primeng/table';
import { AccountRoutingModule } from './account-routing.module';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { TenantBoxComponent } from './components/tenant-box/tenant-box.component';
import { Options } from './models/options';
import { ACCOUNT_OPTIONS, optionsFactory } from './tokens/options.token';

@NgModule({
  declarations: [LoginComponent, RegisterComponent, TenantBoxComponent],
  imports: [CoreModule, AccountRoutingModule, ThemeSharedModule, TableModule, NgbDropdownModule, NgxValidateCoreModule],
  exports: [],
})
export class AccountModule {}

export function AccountProviders(options = {} as Options): Provider[] {
  return [
    { provide: ACCOUNT_OPTIONS, useValue: options },
    {
      provide: 'ACCOUNT_OPTIONS',
      useFactory: optionsFactory,
      deps: [ACCOUNT_OPTIONS],
    },
  ];
}
