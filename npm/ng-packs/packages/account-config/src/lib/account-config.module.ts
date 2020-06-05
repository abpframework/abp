import { CoreModule, noop } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { APP_INITIALIZER, InjectionToken, ModuleWithProviders, NgModule } from '@angular/core';
import { AccountConfigService } from './services/account-config.service';

export interface AccountConfigOptions {
  redirectUrl?: string;
}

export function accountOptionsFactory(options: AccountConfigOptions) {
  return {
    redirectUrl: '/',
    ...options,
  };
}

export const ACCOUNT_OPTIONS = new InjectionToken('ACCOUNT_OPTIONS');

@NgModule({
  imports: [CoreModule, ThemeSharedModule],
  providers: [{ provide: APP_INITIALIZER, multi: true, deps: [AccountConfigService], useFactory: noop }],
})
export class AccountConfigModule {
  static forRoot(options = {} as AccountConfigOptions): ModuleWithProviders {
    return {
      ngModule: AccountConfigModule,
      providers: [
        { provide: ACCOUNT_OPTIONS, useValue: options },
        {
          provide: 'ACCOUNT_OPTIONS',
          useFactory: accountOptionsFactory,
          deps: [ACCOUNT_OPTIONS],
        },
      ],
    };
  }
}
