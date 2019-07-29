import { CoreModule } from '@abp/ng.core';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { AccountRoutingModule } from './account-routing.module';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { TenantBoxComponent } from './components/tenant-box/tenant-box.component';
import { Options } from './models/options';
import { ACCOUNT_OPTIONS, optionsFactory } from './tokens/options.token';
import { TableModule } from 'primeng/table';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';

@NgModule({
  declarations: [LoginComponent, RegisterComponent, TenantBoxComponent],
  imports: [CoreModule, AccountRoutingModule, ThemeSharedModule, TableModule, NgbDropdownModule, NgxValidateCoreModule],
  exports: [],
})
export class AccountModule {
  static forRoot(options = {} as Options): ModuleWithProviders {
    return {
      ngModule: AccountModule,
      providers: [
        { provide: ACCOUNT_OPTIONS, useValue: options },
        {
          provide: 'ACCOUNT_OPTIONS',
          useFactory: optionsFactory,
          deps: [ACCOUNT_OPTIONS],
        },
      ],
    };
  }
}
